using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using NRegFreeCom;
using RegFreeCom;
using RegFreeCom.Implementations;
using RegFreeCom.Interfaces;


namespace CsComWin32
{
    class Program
    {
        private static ISimpleObject _service;
        private static IntPtr _pointer;

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int Initialize(IntPtr service);


        [STAThread]
        static void Main(string[] args)
        {
            TestSafetyOfDllSearch();
            TestDllDependenciesLoading();

            //var pathToImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "picture.png");
            //IUnknown unknown;
            //var guid = new Guid("7BF80981-BF32-101A-8BBB-00AA00300CAB");
            //var hLoad = NativeMethods.OleLoadPicturePath(pathToImage, IntPtr.Zero, 0, 0,ref guid, out unknown);
            //Debug.Assert(hLoad == SYSTEM_ERROR_CODES.ERROR_SUCCESS);
            //Debug.Assert(unknown !=null);
        
            TestCreateNativeCOM();

            // load native dll
            var loader = new AssemblySystem();
            var module = loader.LoadAnyCpuSubLibrary("RegFreeComNativeConsumer.dll");

            //call dll enty point if needed
            //var main = module.GetDelegate<DEF.DllMain>();
            //bool result = main(module.Handle, FDW_REASONS.DLL_PROCESS_ATTACH, IntPtr.Zero);
            //if (!result) throw new Exception("Failed to init dll");
            
            // push managed COM service into native method
            _service = new SimpleObject();
            _service.FloatProperty = 42;
            _pointer = Marshal.GetIUnknownForObject(_service);
            var initialize = module.GetDelegate<Initialize>();
            int error = initialize(_pointer);
            if (error != 0) throw new Exception();

            // load resource
            var value = module.LoadCompiledResource(5435);
            Console.WriteLine(value);
        }

        private static void TestSafetyOfDllSearch()
        {
            var t = new Thread(() =>
                {
                    var r = NativeMethods.SetDllDirectory(AppDomain.CurrentDomain.BaseDirectory);
                    Debug.Assert(r == true);
                });
            t.Start();
            t.Join();
            //NativeMethods.SetDllDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        private static void TestCreateNativeCOM()
        {
            var nativeImplementation = new Guid("538ECD5D-8A57-4F1C-AEB1-EBC425641F0B");
            var obj = NRegFreeCom.ActivationContext.CreateInstanceWithManifest(nativeImplementation,
                                                                               "Win32/RegFreeComNativeImplementer.dll.manifest");
            var obj2 = obj as ILoadedByManagedImplementedByNative;
            var str = obj2.Get();
            Console.WriteLine(str);
        }

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate short fnNativeLibraryConsumer();
        private static void TestDllDependenciesLoading()
        {
       
            var buffer = new StringBuilder(byte.MaxValue);
            
            var rl = NativeMethods.GetDllDirectory(buffer.Capacity,  buffer);
            var result = buffer.ToString();
            if (rl == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
            var loader = new AssemblySystem();
            loader.AddDllDirectoryToSearchPath = false;
            var module = loader.LoadAnyCpuSubLibrary("NativeLibraryConsumer.dll");
            var fn = module.GetDelegate<fnNativeLibraryConsumer>();
            Debug.Assert(42 == fn());
        }
    }
}