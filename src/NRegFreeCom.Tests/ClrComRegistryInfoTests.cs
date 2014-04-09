using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [ComVisible(true)]
    public class SimplestComClass { }

    [TestFixture]
    public class ClrComRegistryInfoTests
    {

        public abstract class AbstractClass {}


        public class NoComVisibleAttrClass { }

        [ComVisible(true)]
        [ProgId("MyProgId")]
        public class ProgIdDefinedClass{}

        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Create_abstract_error()
        {
            ClrComRegistryInfo.Create(typeof (AbstractClass));
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Create_noComVisibleAttr_error()
        {
            ClrComRegistryInfo.Create(typeof(NoComVisibleAttrClass));
        }

        [Test]
        
        public void Create_progId_OK()
        {
          var result =   ClrComRegistryInfo.Create(typeof(ProgIdDefinedClass));
           StringAssert.AreEqualIgnoringCase("MyProgId",result.ProgId);
        }

        [Test]
        public void Create_assemblyPathAndTypeName_OK()
        {
            var type = "NRegFreeCom.Tests.SimplestComClass";
            var created = ClrComRegistryInfo.Create(System.Reflection.Assembly.GetExecutingAssembly().Location, type);
            StringAssert.Contains(type,created.Class);
        }
    }
}
