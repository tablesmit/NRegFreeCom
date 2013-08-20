using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using EasyHook;
using NRegFreeCom;
using NUnit.Framework;
using RegFreeCom;
using RegFreeCom.Implementations;
using RegFreeCom.Interfaces;
using ActivationContext = NRegFreeCom.ActivationContext;
using Assembly = NRegFreeCom.Assembly;


namespace CsComWin32
{

    public class Program : EasyHook.IEntryPoint
    {
        private static ISimpleObject _service;
        private static IntPtr _pointer;
        private static bool _wasHooked;

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int Initialize(IntPtr service);

        
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int ReadRegistry();

        

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

            //hook
     
            var CreateFileHook = LocalHook.Create(
               LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
               new DCreateFile(CreateFile_Hooked),
               p);
            CreateFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            var loader = new AssemblySystem();
            var nativeLibrary = loader.LoadFrom(loader.GetAnyCpuPath(loader.BaseDirectory), "NativeLibrary");
            var registry = nativeLibrary.GetDelegate<ReadRegistry>();
            registry.Invoke();

           
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
          [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate IntPtr DCreateFile(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        // just use a P-Invoke implementation to get native API access
        // from C# (this step is not necessary for C++.NET)
        [DllImport("kernel32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr CreateFile(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        // this is where we are intercepting all file accesses!
        static IntPtr CreateFile_Hooked(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile)
        {
            _wasHooked = true;

            return CreateFile(InFileName,
                              InDesiredAccess,
                              InShareMode,
                              InSecurityAttributes,
                              InCreationDisposition,
                              InFlagsAndAttributes,
                              InTemplateFile);
        }
    


        public class RegistyEasyHook:EasyHook.IEntryPoint 
        {
            
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