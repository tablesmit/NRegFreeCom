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
        public void WpfRunInvoke()
        {
            System.Windows.Threading.Dispatcher wpfDisp = null;
            var created = new ManualResetEvent(false);
            var t = new Thread(x =>
            {
                wpfDisp = System.Windows.Threading.Dispatcher.CurrentDispatcher;
                created.Set();
                System.Windows.Threading.Dispatcher.Run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            Thread.Sleep(50);//wait for started
            created.WaitOne();
            bool wasAct = false;
            Action act = () =>
                {
                    wasAct = true;
                };
            wpfDisp.Invoke(act);

            wpfDisp.InvokeShutdown();
            Assert.That(wasAct, Is.True);
        }

        [Test]
        public void RunInvoke()
        {
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
            Thread.Sleep(50);//wait for started
            created.WaitOne();
            bool wasAct = false;
            Action act = () =>
            {
                wasAct = true;
            };
            disp.Invoke(act);

            disp.InvokeShutdown();
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

