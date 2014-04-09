using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using NUnit.Framework;

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

        [Test]
        public void User_RegisterInProcSeverAndAvoidRedirection()
        {
            var type = typeof(RuntimeRegServer);
             if (RegAsm.IsWoW64RedirectionOn)
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
