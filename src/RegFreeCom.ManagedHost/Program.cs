using System;

using System.Runtime.InteropServices;

using NRegFreeCom;
using NUnit.Framework;

using RegFreeCom.Implementations;
using RegFreeCom.Interfaces;
using ActivationContext = NRegFreeCom.ActivationContext;
using Assembly = NRegFreeCom.Assembly;


namespace CsComWin32
{

    public class Program 
    {
        private static ISimpleObject _service;
        private static IntPtr _pointer;
        private static bool _wasHooked;

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int Initialize(IntPtr service);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int GetComInterface( // marshal generic array of items
        [MarshalAs(UnmanagedType.SafeArray,
           SafeArraySubType = VarEnum.VT_UNKNOWN
            )]out object[] retval)
            ;


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
            _service = new RegFreeSimpleObject();
            _service.FloatProperty = 42;
            _pointer = Marshal.GetIUnknownForObject(_service);
            var initialize = module.GetDelegate<Initialize>();
            int error = initialize(_pointer);
            if (error != 0) throw new Exception();

            EnsureGC();
            _service.RaiseEnsureGCIsNotObstacle();

            // return string from C

            IntPtr strPtr = IntPtr.Zero;
            module.GetDelegate<GetStringResult>()(out strPtr);
            var str = Marshal.PtrToStringUni(strPtr);
            Marshal.FreeBSTR(strPtr);
            Console.WriteLine(str);

            // return array of COM services which return pointers
            var com = loader.LoadFrom(loader.GetAnyCpuPath(loader.BaseDirectory), "RegFreeComNativeImplementer");
            var servicesGetter = com.GetDelegate<GetComInterface>();
            object[] services;
            servicesGetter(out services);
            var srv = (RegFreeComNativeInterfacesLib.IMyService) services[0];
            IntPtr otherHandle;
            srv.GetSomeHanlde(out otherHandle);
            var srvRepeat = (RegFreeComNativeInterfacesLib.IMyService)Marshal.GetObjectForIUnknown(otherHandle);
            // pointer to itself was used to simplify testing
            Assert.AreEqual(services[0],srvRepeat);



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