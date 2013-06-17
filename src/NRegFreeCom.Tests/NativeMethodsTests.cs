using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class NativeMethodsTests
    {
        [Test]
        public void TestLoadComImage()
        {
            var pathToImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "picture.png");
            IUnknown unknown;
            var guid = new Guid("7BF80981-BF32-101A-8BBB-00AA00300CAB");
            var hLoad =  Ole.NativeMethods.OleLoadPicturePath(pathToImage, IntPtr.Zero, 0, 0, ref guid, out unknown);
            Assert.IsTrue(hLoad == SYSTEM_ERROR_CODES.ERROR_SUCCESS);
            Assert.IsTrue(unknown != null);
        }
        [Test]
        public void TestSafetyOfDllSearch()
        {
            var t = new Thread(() =>
            {
                var r = NativeMethods.SetDllDirectory(AppDomain.CurrentDomain.BaseDirectory);
                Debug.Assert(r == true);
            });
            t.Start();
            t.Join();

            //NativeMethods.SetDllDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var buffer = new StringBuilder(byte.MaxValue);
            var rl = NativeMethods.GetDllDirectory(buffer.Capacity, buffer);
            var result = buffer.ToString();
            if (rl < 0) throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}
