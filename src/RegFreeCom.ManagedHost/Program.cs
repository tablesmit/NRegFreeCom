using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using NRegFreeCom;
using NUnit.Framework;
using RegFreeCom;
using RegFreeCom.Implementations;
using RegFreeCom.Interfaces;
using ActivationContext = NRegFreeCom.ActivationContext;
using Assembly = NRegFreeCom.Assembly;


namespace CsComWin32
{

    class Program
    {
        private static ISimpleObject _service;
        private static IntPtr _pointer;

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int Initialize(IntPtr service);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int GetComInterface(out IntPtr service);


        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate void GetStringResult(
            //[Out][MarshalAs(UnmanagedType.LPWStr)] 
        out IntPtr str
        );

        [STAThread]
        static void Main(string[] args)
        {

            var p = new Program();

            var loader = new AssemblySystem();
            var module = loader.LoadFrom(loader.GetAnyCpuPath(loader.BaseDirectory), "RegFreeComNativeConsumer.dll");

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

            EnsureGC();
            _service.RaiseEnsureGCIsNotObstacle();

            // load resource
            var value = module.LoadCompiledResource(5435);
            Console.WriteLine(value);

            // return string from C

            IntPtr strPtr = IntPtr.Zero;
            module.GetDelegate<GetStringResult>()(out strPtr);
            var str = Marshal.PtrToStringUni(strPtr);
            Marshal.FreeBSTR(strPtr);
            Console.WriteLine(str);

            var com = loader.LoadFrom(loader.GetAnyCpuPath(loader.BaseDirectory), "RegFreeComNativeImplementer");
            var servicesGetter = com.GetDelegate<GetComInterface>();
            IntPtr services;
            servicesGetter(out services);
           var srv = (RegFreeComNativeInterfacesLib.IMyService) Marshal.GetObjectForIUnknown(services);
            IntPtr otherHandle;
            srv.GetSomeHanlde(out otherHandle);
            var srvRepeat = (RegFreeComNativeInterfacesLib.IMyService)Marshal.GetObjectForIUnknown(otherHandle);
            Console.ReadKey();
        }

        private static void EnsureGC()
        {
            GC.Collect();
            GC.WaitForFullGCComplete(200);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }







    }
}