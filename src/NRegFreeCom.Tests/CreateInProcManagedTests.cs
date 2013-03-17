using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

using RegFreeCom.Interfaces;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class CreateInProcManagedTests
    {
        [Test]
        public static void CreateInProcServerByManifest()
        {
            var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.Implementations.dll.manifest");
            var guid = new Guid(RegFreeComIds.CLSID);
            var obj = ActivationContext.CreateInstanceWithManifest(guid, path);
            var inf = (IRegFreeCom)obj;
            var result = inf.GetString(42);
            Assert.IsNotNullOrEmpty(result);
            Assert.IsTrue(result.Contains("42"));
        }

        [Test(Description = "Ensures that class is not registered and does not works without manifest")]
        [ExpectedException("System.Runtime.InteropServices.COMException")]
        public static void CreateInProcServerWithoutManifest()
        {
            var guid = new Guid(RegFreeComIds.CLSID);
            Type T = Type.GetTypeFromCLSID(guid);
            var comob = System.Activator.CreateInstance(T);
        }

        [Test()]
        public static void CreateInProcServerWithManifestByProgId()
        {
            var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.Implementations.dll.manifest");
            object obj = null;
            ActivationContext.UsingManifestDo(path, () =>
                {
                    var type = System.Type.GetTypeFromProgID("RegFreeCom.Implementations.RegFreeLocalServer");
                    obj = Activator.CreateInstance(type);

                });
            var inf = (IRegFreeCom)obj;
            Assert.IsNotNull(inf);
        }

    }
}
