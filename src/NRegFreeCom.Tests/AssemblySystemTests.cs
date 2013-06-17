using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [NUnit.Framework.TestFixture]
    public class AssemblySystemTests
    {

        
        [Test]
        public void LoadingDll_noSuchDll_clrAndNativeErrorAreTheSame()
        {
            Exception clrEx = null;
            var noSuchDll = Guid.NewGuid().ToString("N") + "_" + DateTime.Now.Ticks + ".dll";
            try
            {
                System.Reflection.Assembly.LoadFrom(noSuchDll);
            }
            catch (Exception ex)
            {
                clrEx = ex;
            }
            Exception nativeEx = null;
            try
            {
                var native = new NRegFreeCom.AssemblySystem();
                native.LoadFrom(noSuchDll);
            }
            catch (Exception ex)
            {
                nativeEx = ex;
            }
            Assert.AreEqual(clrEx.GetType(),nativeEx.GetType());

        }


        [Test]
        public void LoadingDll_notDll_clrAndNativeErrorAreTheSame()
        {
            Exception clrEx = null;
            var notDll = Guid.NewGuid().ToString("N") + "_" + DateTime.Now.Ticks + ".dll";
            File.Create(notDll).Close();
            try
            {
                System.Reflection.Assembly.LoadFrom(notDll);
            }
            catch (Exception ex)
            {
                clrEx = ex;
            }
            Exception nativeEx = null;
            try
            {
                var native = new NRegFreeCom.AssemblySystem();
                native.LoadFrom(notDll);
            }
            catch (Exception ex)
            {
                nativeEx = ex;
            }
            File.Delete(notDll);
            Assert.AreEqual(clrEx.GetType(), nativeEx.GetType());
            

        }


        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate Int32 fnNativeLibraryConsumer(
            // marshal generic array of items
        [MarshalAs(UnmanagedType.SafeArray,
           SafeArraySubType = VarEnum.VT_VARIANT
            )]out object[] retval);

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int fnNativeLibrary();


        [Test]
        public void TestDllDependenciesLoading()
        {
            var loader = new AssemblySystem();
            var anyCpu = loader.GetAnyCpuPath(loader.BaseDirectory);
            loader.AddSearchPath(anyCpu);
            var module = loader.LoadFrom(anyCpu, "NativeLibraryConsumer.dll");
            var fn = module.GetDelegate<fnNativeLibraryConsumer>();
            object[] retval;

            Assert.IsTrue(42 == fn(out retval));
        }



        [Test]
        public void Referenced()
        {
            var server = this.GetType().Assembly.GetReferencedAssemblies().Where(x => x.Name.Contains("RegFreeCom.Implementations.dll"));
            Assert.AreEqual(0,server.Count(),"Should not reference COM server dll directly");
        }

        [Test]
        public void TesNativeInvoke()
        {
            var module = LoadDll();
            var fn = module.GetDelegate<fnNativeLibrary>();
            var fnString = module.GetDelegate<fnNativeLibrary>("fnNativeLibrary");
            Assert.IsTrue(42 == fn());
            Assert.IsTrue(42 == fnString());
        }

        [Test]
        [ExpectedException(typeof(EntryPointNotFoundException))]
        public void TesNativeInvoke_noSuchFunction()
        {
            var module = LoadDll();

            var fn = module.GetDelegate<fnNativeLibrary>("noSuchFunction" + DateTime.Now.Ticks);
            Assert.IsTrue(42 == fn());
   
        }

        [Test]
        public void TestDllsLoading()
        {
             LoadDll();
        }

        private static Assembly LoadDll()
        {
            var loader = new AssemblySystem();
            var anyCpu = loader.GetAnyCpuPath(loader.BaseDirectory);
            loader.AddSearchPath(anyCpu);
            Assembly module = loader.LoadFrom(anyCpu, "NativeLibrary.dll");
            return module;
        }

        [Test]
        public void TestLoadeAndDisposeNativeDll()
        {
            var loader = new AssemblySystem();
            var anyCpu = loader.GetAnyCpuPath(loader.BaseDirectory);
            var module = loader.LoadFrom(anyCpu, "RegFreeComNativeConsumer.dll");
            module.Dispose();

        }

 


    }
}
