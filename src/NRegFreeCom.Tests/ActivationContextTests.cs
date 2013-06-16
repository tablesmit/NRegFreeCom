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
    public class ActivationContextTests
    {
        [Test]
        public  void CreateInProcServerByManifest()
        {
            var path = Path.Combine(Environment.CurrentDirectory, @"RegFreeCom.Implementations.dll.manifest");
            var guid = new Guid(RegFreeComIds.CLSID);
            var obj = ActivationContext.CreateInstanceWithManifest(guid, path);
            var inf = (IRegFreeCom)obj;
            var result = inf.Answer();

            Assert.IsTrue(result == 42);
        }

 

        [Test(Description = "Ensures that class is not registered and does not works without manifest")]
        [ExpectedException(typeof(COMException))]
        public  void CreateInProcServerWithoutManifest()
        {
            var guid = new Guid(RegFreeComIds.CLSID);
            Type T = Type.GetTypeFromCLSID(guid);
            var comob = System.Activator.CreateInstance(T);
        }

        [Test()]
        public  void CreateInProcServerWithManifestByProgId()
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

        [Test]
        public void TestCreateNativeCOM()
        {
            var nativeImplementation = new Guid("538ECD5D-8A57-4F1C-AEB1-EBC425641F0B");
            var obj = NRegFreeCom.ActivationContext.CreateInstanceWithManifest(nativeImplementation,
                                                                               "Win32/RegFreeComNativeImplementer.dll.manifest");
            var obj2 = obj as ILoadedByManagedImplementedByNative;
            var str = obj2.Get();
            Console.WriteLine(str);
        }

    }
}
