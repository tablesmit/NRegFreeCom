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
    public class ComClrInfoFactoryTests
    {

        public abstract class AbstractClass {}


        public class NoComVisibleAttrClass { }

       [ComVisible(true)]
       [Guid("A3A62CBD-2801-4203-BF50-7269D380AB5A")]
       public interface ISimplestComClass { }
        
        [ComVisible(true)]
        [ProgId("MyProgId")]
        public class ProgIdDefinedClass{}

        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CreateClass_abstract_error()
        {
            ComClrInfoFactory.CreateClass(typeof (AbstractClass));
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CreateClass_noComVisibleAttr_error()
        {
            ComClrInfoFactory.CreateClass(typeof(NoComVisibleAttrClass));
        }

        [Test]
        
        public void CreateClass_progId_OK()
        {
          var result =  ComClrInfoFactory.CreateClass(typeof(ProgIdDefinedClass));
           StringAssert.AreEqualIgnoringCase("MyProgId",result.ProgId);
        }

        
        [Test]
        public void CreateClass_simpleComClass_hasRuntime()
        {
            var created =  ComClrInfoFactory.CreateClass(typeof(SimplestComClass));
            Assert.IsNotNullOrEmpty(created.RuntimeVersion);
        }
    
        [Test]
        public void CreateClass_assemblyPathAndTypeName_OK()
        {
            var type = "NRegFreeCom.Tests.SimplestComClass";
            var asm = System.Reflection.Assembly.ReflectionOnlyLoadFrom(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var created =  ComClrInfoFactory.CreateClass(asm, type);
            StringAssert.Contains(type,created.Class);
        }
   
        [Test]
        public void CreateInterface_assemblyPathAndTypeName_OK()
        {
           var type = typeof(ISimplestComClass);
           var created =  ComClrInfoFactory.CreateInterface(type);
           Assert.IsNotNull(created.TypeLib);
    
        }
    }
}
