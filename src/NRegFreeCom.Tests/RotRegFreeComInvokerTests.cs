using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NRegFreeCom.Interop.ComTypes;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{



    [TestFixture]
    public class RotRegFreeComInvokerTests
    {
        [ComVisible(true)]
        public interface IMyComObject
        {
        }
        [ComVisible(true)]
        [System.Runtime.InteropServices.ComDefaultInterface(typeof(IMyComObject))]
        public class MyComObject:IMyComObject
        {
            
        }

        [Test]
        public void Test()
        {
            var com = new MyComObject();
            var proxy = RotRegFreeComInvoker.ProxyInterface<IMyComObject>(com);
            var casted = proxy is IMyComObject;
            var castedToUnkown = proxy is IUnknown;
            Assert.IsTrue(casted);
        }
    }


}
