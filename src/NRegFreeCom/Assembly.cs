using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;


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
            var function = Marshal.GetDelegateForFunctionPointer(fPtr, typeof(T)) ;
            nativeDelegate = function as T;
            return true;
        }


        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException("_hModule");
        }

        public IntPtr LoadCompiledResource(uint name)
        {
            var loaded = FindLoadLock(_hModule, name, RESOURCE_TYPES.RCDATA);
            return loaded;
        }



        private IntPtr FindLoadLock(IntPtr hModule, uint name, uint type)
        {
            IntPtr hResource;  //handle to resource
            IntPtr pResource;  //pointer to resource in memory
            hResource = NativeMethods.FindResource(hModule, name, RESOURCE_TYPES.RCDATA);
            if (hResource.ToInt32() == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            hResource = NativeMethods.LoadResource(hModule, hResource);
            if (hResource.ToInt32() == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            pResource = NativeMethods.LockResource(hResource);
            if (pResource.ToInt32() == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return pResource;
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