
using System.Runtime.InteropServices;
using Microsoft.Win32;
using NUnit.Framework;
using UnitWrappers.System;
using UnitWrappers;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class RegAsmTests
    {

        [ComVisible(true)]
        public class NeverRegistered{}

        [Test]
        public void User_RegisterInProcSever()
        {
            var type = typeof (RuntimeRegServer);
            RegAsm.User.RegisterInProcServer(type);
        }

        private UnitWrappers.System.IEnvironment Environment = new EnvironmentWrap();

        [Test]
        public void User_RegisterInProcSeverAndAvoidRedirection()
        {
            var type = typeof(RuntimeRegServer);
            if (Environment.IsWoW64RedirectionOn())
              RegAsm.User.RegisterInProcServer(type,RegistryView.Registry64);// 32 bit process writes to shared registration instead of redirected
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
