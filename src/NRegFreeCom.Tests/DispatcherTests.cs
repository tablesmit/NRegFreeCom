using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class DispatcherTests
    {

        [Test]
        public void WpfRunShutdown()
        {
            System.Windows.Threading.Dispatcher wpfDisp = null;
            var t = new Thread(x =>
                {
                    wpfDisp = System.Windows.Threading.Dispatcher.CurrentDispatcher;
                    System.Windows.Threading.Dispatcher.Run();
                });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            bool waitRun = t.Join(TimeSpan.FromMilliseconds(200));
            Assert.False(wpfDisp.HasShutdownFinished);
            wpfDisp.InvokeShutdown();

            bool waitShutdown = t.Join(TimeSpan.FromMilliseconds(50));
            Assert.False(waitRun);
            Assert.True(wpfDisp.HasShutdownFinished);
            Assert.True(waitShutdown);
        }


        [Test]
        public void GetAndRunWpfDispatcher_invokeAndShutdown_invocationInDispatcherDone()
        {
            System.Windows.Threading.Dispatcher disp = null;
            var created = new ManualResetEvent(false);
            var t = new Thread(x =>
            {
                disp = System.Windows.Threading.Dispatcher.CurrentDispatcher;
                created.Set();
                System.Windows.Threading.Dispatcher.Run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            created.WaitOne();

            int threadId = -1;
            bool wasAct = false;
            Action act = () =>
            {
                wasAct = true;
                threadId = Thread.CurrentThread.ManagedThreadId;
            };
            disp.Invoke(act);
            disp.InvokeShutdown();

            Assert.AreEqual(threadId, t.ManagedThreadId);
            Assert.That(wasAct, Is.True);
        }

        [Test]
        [Description("Tests basic workflow of custom Dispatcher")]
        public void GetAndRunDispatcher_invokeAndShutdown_invocationInDispatcherDone()
        {
            //start custom Dispatcher for Windows Message Pump
            NRegFreeCom.IDispatcher disp = null;
            var created = new ManualResetEvent(false);
            var t = new Thread(x =>
            {
                disp = NRegFreeCom.Dispatcher.CurrentDispatcher;
                created.Set();
                NRegFreeCom.Dispatcher.Run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            created.WaitOne();

            // run custom Dispatcher
            int threadId = -1;
            bool wasAct = false;
            Action act = () =>
            {
                wasAct = true;
                threadId = Thread.CurrentThread.ManagedThreadId;
            };
            disp.Invoke(act);
            disp.InvokeShutdown();

            // invocation in Dispatcher thread was done
            Assert.AreEqual(threadId, t.ManagedThreadId);
            Assert.That(wasAct, Is.True);
        }

        [Test]
        public void RunShutdown()
        {
            NRegFreeCom.IDispatcher disp = null;
            var t = new Thread(x =>
            {
                disp = NRegFreeCom.Dispatcher.CurrentDispatcher;
                NRegFreeCom.Dispatcher.Run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            bool waitRun = t.Join(TimeSpan.FromMilliseconds(200));
            Assert.False(disp.HasShutdownFinished);
            disp.InvokeShutdown();

            bool waitShutdown = t.Join(TimeSpan.FromMilliseconds(50));
            Assert.False(waitRun);
            Assert.True(disp.HasShutdownFinished);
            Assert.True(waitShutdown);
        }
    }
}

