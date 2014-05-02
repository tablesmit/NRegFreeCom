using System;
using System.Threading;
using NRegFreeCom.Threading;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class WaitHandleExtensionsTests
    {
        [Test]
        public void WaitOneNonAlertable_signaled_true()
        {
            var e = new ManualResetEvent(true);
            bool result = e.WaitOneNonAlertable(10);
            Assert.IsTrue(result);
        }

        [Test]
        public void WaitOneNonAlertable_nonSignaled_false()
        {
            var e = new ManualResetEvent(false);
            
            bool result = e.WaitOneNonAlertable(10);
            
            Assert.IsFalse(result);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void WaitOneNonAlertable_disposed_error()
        {
            var e = new ManualResetEvent(true);
            e.Dispose();

            e.WaitOneNonAlertable(10);
        }


        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WaitOneNonAlertable_wrongTimeout_error()
        {
            var e = new ManualResetEvent(true);
     
            e.WaitOneNonAlertable(-2);
        }

    }
}
