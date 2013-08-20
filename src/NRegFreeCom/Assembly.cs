using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using NRegFreeCom.Interop;


namespace NRegFreeCom
{
    public class Assembly : IAssembly
    {
        private readonly IntPtr _hModule;
        private bool _disposed;
        private string _name;
        private string _location;

        internal Assembly(IntPtr hModule, string name, string location)
        {
            Debug.Assert(hModule != IntPtr.Zero);
            _hModule = hModule;
            _name = name;
            _location = location;
        }

        public IntPtr Handle
        {
            get
            {
                ThrowIfDisposed();
                return _hModule;
            }
        }

        public string FullName
        {
            get { return _name; } //TODO: + Version? }

        }

        public string Location
        {
            get { return _location; }
        }

        ///<inheritdoc/>
        public T GetDelegate<T>(string defName = null)
            where T : class,ISerializable, ICloneable // is delegate
        {
            ThrowIfDisposed();
            if (defName == null)
                defName = typeof(T).Name;
            IntPtr fPtr = NativeMethods.GetProcAddress(_hModule, defName);
            if (fPtr == IntPtr.Zero)
            {
                var msg = string.Format("Failed to find {0} function in {1} module", defName, FullName);
                throw new EntryPointNotFoundException(msg, new Win32Exception(Marshal.GetLastWin32Error()));
            }
            var function = Marshal.GetDelegateForFunctionPointer(fPtr, typeof(T));
            return function as T;
        }

        ///<inheritdoc/>
        public bool TryGetDelegate<T>(out T nativeDelegate, string defName = null) where T : class, ISerializable, ICloneable
        {
            ThrowIfDisposed();
            if (defName == null)
                defName = typeof(T).Name;
            IntPtr fPtr = NativeMethods.GetProcAddress(_hModule, defName);
            if (fPtr == IntPtr.Zero)
            {
                nativeDelegate = null;
                return false;
            }
            var function = Marshal.GetDelegateForFunctionPointer(fPtr, typeof(T));
            nativeDelegate = function as T;
            return true;
        }



        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException("_hModule");
        }

        public Stream LoadCompiledResource(uint id)
        {
            // locate resources
            IntPtr hResInfo = NativeMethods.FindResource(_hModule, id, RESOURCE_TYPES.RCDATA);
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return loadResources(hResInfo);
        }

        public Stream LoadCompiledResource(string name)
        {
            // locate resources
            IntPtr hResInfo = NativeMethods.FindResource(_hModule, name, RESOURCE_TYPES.RCDATA);
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return loadResources(hResInfo);
        }

        public Stream LoadResource(uint id, RESOURCE_TYPES type)
        {
            // locate resources
            IntPtr hResInfo = NativeMethods.FindResource(_hModule, id, type);
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return loadResources(hResInfo);
        }

        public Stream LoadResource(string name, RESOURCE_TYPES type)
        {
            IntPtr hResInfo = NativeMethods.FindResource(_hModule, name, type);
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return loadResources(hResInfo);
        }

        public string LoadStringTableResource(uint id)
        {
            var buffer = new StringBuilder(128);

            //NOTE: like Environment.GetEnvironmentVariable - increase initially small buffer
        TRYREAD:
            int readLength = NativeMethods.LoadString(_hModule, id, buffer, buffer.Capacity);

            if (readLength == 0)
            {
                return null;
            }
            if (readLength == buffer.Capacity - 1)
            {
                buffer.Capacity += 2;//TODO: define step more clever, investigate reported last win error
                buffer.Length = 0;
                goto TRYREAD;
            }
            return buffer.ToString();
        }



        private unsafe Stream loadResources(IntPtr hResInfo)
        {
            var sizeOfRes = NativeMethods.SizeofResource(_hModule, hResInfo);
            // get handle to memory pointer of resources
            IntPtr hGLOBAL = NativeMethods.LoadResource(_hModule, hResInfo);
            if (hGLOBAL == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            //pointer to resource in memory
            IntPtr pResource = NativeMethods.LockResource(hGLOBAL);
            if (pResource == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            unsafe
            {
                return new UnmanagedMemoryStream((byte*) pResource.ToPointer(), sizeOfRes);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                NativeMethods.FreeLibrary(_hModule);
            }
        }
    }
}