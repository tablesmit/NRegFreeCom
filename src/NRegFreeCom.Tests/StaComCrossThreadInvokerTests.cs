using System;
using System.Threading;
using NRegFreeCom.Interop;
using NRegFreeCom.Interop.ComTypes;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{


    public class TestMessageFilter : IMessageFilter
    {
        public int HandleInComingCall(uint dwCallType, IntPtr htaskCaller, uint dwTickCount, INTERFACEINFO[] lpInterfaceInfo)
        {
            return 1;
        }

        public int RetryRejectedCall(IntPtr htaskCallee, uint dwTickCount, uint dwRejectType)
        {
            return 1;
        }

        public int MessagePending(IntPtr htaskCallee, uint dwTickCount, uint dwPendingType)
        {
            return 1;
        }
    }

    [TestFixture]
    public class StaComCrossThreadInvokerTests
    {
        [Test]
        public void Create_disposeInOtherThread_error()
        {
            Exception error = null;
            StaComCrossThreadInvoker sut = null;
            var t1 = new Thread(() =>
            {
                sut = new StaComCrossThreadInvoker();
            });
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
            t1.Join();

            var t2 = new Thread(() =>
            {
                try
                {
                    sut.Dispose();
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            });
            t2.SetApartmentState(ApartmentState.STA);
            t2.Start();
            t2.Join();

            Assert.IsInstanceOf<InvalidOperationException>(error);
        }

        [Test]
        public void Create_MtaThread_error()
        {
            Exception error = null;
            var thread = new Thread(() =>
            {
                try
                {
                    using (new StaComCrossThreadInvoker()){}
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            thread.Join();

            Assert.IsInstanceOf<InvalidOperationException>(error);
        }

        [Test]
        public void CreateAndDispose_previousFilterRegistered_previousFilterReturned()
        {
            IMessageFilter noFilterByDefault = null;
            IMessageFilter testFilter = null;
            IMessageFilter testFilterReturned = null;
            var thread = new Thread(() =>
                                        {
                                            testFilter = new TestMessageFilter();
                                            NativeMethods.CoRegisterMessageFilter(testFilter, out noFilterByDefault);
                                            using (new StaComCrossThreadInvoker())
                                            {

                                            }
                                            NativeMethods.CoRegisterMessageFilter(null, out testFilterReturned);
                                        });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.IsNull(noFilterByDefault);
            Assert.AreEqual(testFilter,testFilterReturned);
        }

        [Test]
        public void CreateAndDispose_intermediateFilterRegistredAndLeft_exceptionThrown()
        {
            Exception gotException = null;
            IMessageFilter intermediateFilter = null;
            var thread = new Thread(() =>
            {
                intermediateFilter = new TestMessageFilter();
                try
                {
                    using (new StaComCrossThreadInvoker())
                    {
                        IMessageFilter commonFilter = null;
                        NativeMethods.CoRegisterMessageFilter(intermediateFilter, out commonFilter);
                    }
                }
                catch (Exception ex)
                {
                    gotException = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.IsInstanceOf<InvalidOperationException>(gotException);

        }

        [Test]
        public void CreateAndDispose_noFilterWas_noFilterAfter()
        {
            IMessageFilter filterAfter = null;
            var thread = new Thread(() =>
            {
                using (new StaComCrossThreadInvoker())
                {

                }
                NativeMethods.CoRegisterMessageFilter(null, out filterAfter);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.IsNull(filterAfter);

        }

        public class FakeComServer
        {
            public static int LastRetryResult = int.MinValue;// no such value should be returned ever

            /// <summary>
            /// Imitates COM busy for some time.
            /// </summary>
            /// <param name="messageFilter"></param>
            /// <param name="comBusyTime"></param>
            public static void RetryRejectedCall(IMessageFilter messageFilter, uint comBusyTime)
            {
                LastRetryResult = messageFilter.RetryRejectedCall(IntPtr.Zero, comBusyTime, 0);
            }
        }

        [Test]
        public void CreateAndDispose_smallMaximumTotalWaitTimeAndVeryBusyComServer_Canceled()
        {
            var thread = new Thread(() =>
            {
                using (var invoker = new StaComCrossThreadInvoker(1000))
                {
                    FakeComServer.RetryRejectedCall(invoker, 1001);
                    // if really call to COM object here - could throw exceptions
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            Assert.AreEqual(StaComCrossThreadInvoker.CANCEL, FakeComServer.LastRetryResult);
  
        }


        internal class CustomeStaComCrossThreadInvoker:StaComCrossThreadInvoker
        {
            public bool ShouldRetryCalled;

            protected override bool ShouldRetry(uint elpasedTotalWaitTimeInMilliseconds)
            {
                ShouldRetryCalled = true;
                return base.ShouldRetry(elpasedTotalWaitTimeInMilliseconds);
            }
        }

        [Test]
        public void CreateAndDispose_serverRejectsCall_ShouldRetryAsked()
        {
            CustomeStaComCrossThreadInvoker invoker = null;
            var thread = new Thread(() =>
                                        {
                                            using (invoker = new CustomeStaComCrossThreadInvoker())
                                            {
                                                FakeComServer.RetryRejectedCall(invoker,StaComCrossThreadInvoker.DEFAULT_RETRY_TIMEOUT+1000);
                                            }
                                        });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            Assert.IsTrue(invoker.ShouldRetryCalled);

        }
    }


}
