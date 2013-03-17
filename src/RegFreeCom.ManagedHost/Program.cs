using System;
using System.Reflection;
using System.Runtime.InteropServices;
using NRegFreeCom;
using RegFreeCom;
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
            var loader = new AssemblySystem();
            var module = loader.LoadAnyCpuSubLibrary("RegFreeComNativeConsumer.dll");

            //
            var nativeImplementation = new Guid("538ECD5D-8A57-4F1C-AEB1-EBC425641F0B");
            var obj = NRegFreeCom.ActivationContext.CreateInstanceWithManifest(nativeImplementation, "Win32/RegFreeComNativeImplementer.dll.manifest");
            var obj2 = obj as ILoadedByManagedImplementedByNative;
            var qwe = obj.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.Public);
            var m = obj.GetType().GetMethods();
            var str = obj2.Get();
     
                _service = new SimpleObject();
                _service.FloatProperty = 42;
                _pointer = Marshal.GetIUnknownForObject(_service);
    
            var initialize = module.GetDelegate<Initialize>();
            int error = initialize(_pointer);
            if (error != 0) throw new Exception();
            var value = module.LoadCompiledResource(5435);
            Console.WriteLine(value);

        }


    }
}