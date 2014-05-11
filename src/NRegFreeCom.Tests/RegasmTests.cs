
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using NUnit.Framework;


namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class RegAsmTests
    {
        [ComVisible(true)]
        [Guid("CE67C023-1DCC-47FF-BD80-D3E57A4D9044")]
        public enum TraceEventType
        {
            Critical = System.Diagnostics.TraceEventType.Critical,
            Error = System.Diagnostics.TraceEventType.Error,
            Warning = System.Diagnostics.TraceEventType.Warning,
            Information = System.Diagnostics.TraceEventType.Information,
            Verbose = System.Diagnostics.TraceEventType.Verbose,
            Start = System.Diagnostics.TraceEventType.Start,
            Stop = System.Diagnostics.TraceEventType.Stop,
            Suspend = System.Diagnostics.TraceEventType.Suspend,
            Resume = System.Diagnostics.TraceEventType.Resume,
            Transfer = System.Diagnostics.TraceEventType.Transfer
        }


        [Guid("4BF1AC16-D5C9-4103-83FD-7E40FC7B88B2")]
        [ComVisible(true)]
        public interface INeverRegistered { }

        [ComVisible(true)]
        [Guid("73FC8D60-5EC3-4B1E-959E-F114C86C9565")]
        public class NeverRegistered{}

        [Test]
        public void User_RegisterInProcSever()
        {
            var type = typeof (RuntimeRegServer);
            RegAsm.User.RegisterInProcServer(type);
        }


		
		
        /// <summary>
        /// If this is true, then some registry calls are redirected.
        /// </summary>
        /// <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/aa384232.aspx"/>
        public static bool IsWoW64RedirectionOn
        {
		  get 
		   {	 
        	return Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess;
			}
		}
		
        [Test]
        public void User_RegisterInProcSeverAndAvoidRedirection()
        {
            var type = typeof(RuntimeRegServer);
            if (IsWoW64RedirectionOn)
              RegAsm.User.RegisterInProcServer(type,RegistryView.Registry64);// 32 bit process writes to shared registration instead of redirected
        }


 

        [Test]
        public void User_RegisterEnumAsRecord_Ok()
        {
            var type = typeof(TraceEventType);
            RegAsm.User.RegisterRecord(type, RegistryView.Default);
        }

        [Test]
        public void User_RegisterInterface_Ok()
        {
            var type = typeof(INeverRegistered);
            RegAsm.User.RegisterInterface(type, RegistryView.Default);
        }
        
        [Test]
        public void User_RegisterTypeLib_Ok()
        {
           var asm = System.Reflection.Assembly.GetExecutingAssembly();
            RegAsm.User.RegisterTypeLib(asm, RegistryView.Default);
        }



        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void User_RegisterClassAsRecord_error()
        {
            var type = typeof(NeverRegistered);
            RegAsm.User.RegisterRecord(type, RegistryView.Default);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void User_RegisterInterfaceAsClass_error()
        {
            var type = typeof(INeverRegistered);
            RegAsm.User.RegisterInProcServer(type, RegistryView.Default);
        }

        [Test]
        public void User_unregisterNeverRegistered_nothing()
        {
            var type = typeof(NeverRegistered);
            RegAsm.User.UnregisterInProcServer(type);
        }


        [Test]
        public void User_UnregisterInProcSever()
        {
            var type = typeof(RuntimeRegServer);
            RegAsm.User.UnregisterInProcServer(type);
        }


    }
}
