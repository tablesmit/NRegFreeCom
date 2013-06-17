using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace NRegFreeCom
{
    ///<summary>
    /// Rejected COM calls will retry automatically in current thread when instance of this object is used.
    /// </summary>
    ///<example>   
    /// Clients of STA COM servers (Excel,Word,etc.) should use it before calling any methods from thread other then in which COM was created.
    /// <code>
    /// using (new StaComCrossThreadInvoker())
    /// {     
    ///     ...
    /// }
    /// </code>
    /// </example>
    ///<remarks>
    /// Provides <see cref="IMessageFilter"/> implementation with interface oriented on .NET CLIENTS to COM STA SERVERS only.
    /// </remarks>
    ///<seealso href="http://blogs.msdn.com/b/andreww/archive/2008/11/19/implementing-imessagefilter-in-an-office-add-in.aspx"/>
    /// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms693740.aspx"/>
    public class StaComCrossThreadInvoker : IMessageFilter, IDisposable
    {
        private IMessageFilter _oldFilter;
        private bool _disposed;
		private uint _maximumTotalWaitTimeInMilliseconds;

        public const int DEFAULT_RETRY_TIMEOUT = 1;
        public const int CANCEL = -1;
        public const int S_OK = 0x0000;

        /// <summary>
        /// <see cref="Thread.ManagedThreadId"/> were this object was created, can be used and should be disposed.
        /// </summary>
        public int AffinedThreadId { get; private set; }

        /// <summary>
        /// Creates and registers this filter.
        /// </summary>
        public StaComCrossThreadInvoker()
			: this(uint.MaxValue)
        {
        }

		/// <summary>
		/// Creates and registers this filter.
		/// </summary>
		/// <param name="maximumTotalWaitTimeInMilliseconds">
        /// Number of milliseconds before message filter stops spin waiting call to finish, call canceled and COM exceptions popup.
		/// </param>
		public StaComCrossThreadInvoker(uint maximumTotalWaitTimeInMilliseconds)
		{
		    var t = Thread.CurrentThread;
            if (t.GetApartmentState() != ApartmentState.STA) throw new InvalidOperationException("StaComCrossThreadInvoker can be used only from STA thread.");
            AffinedThreadId = t.ManagedThreadId;
			_maximumTotalWaitTimeInMilliseconds = maximumTotalWaitTimeInMilliseconds;
			_oldFilter = null;
			int hr = NativeMethods.CoRegisterMessageFilter(this, out _oldFilter);
            if (hr != S_OK) throw new COMException("Failed to create message filter",hr);
		}

        /// <summary>
        /// Unregisters common filter and returns back previous one.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                if (AffinedThreadId != Thread.CurrentThread.ManagedThreadId) throw new InvalidOperationException("Trying to dispose in other thread than object was created.");
                IMessageFilter shouldBeCommonFilter = null;
                int hr = NativeMethods.CoRegisterMessageFilter(_oldFilter, out shouldBeCommonFilter);
                if (shouldBeCommonFilter != this) throw new InvalidOperationException("Trying to dispose unknown message filter. Other message filter was registered inside common filter without returning back common filter after usage.");
                if (hr != S_OK) throw new COMException("Failed to dispose message filter", hr);
                _disposed = true;
            }
        }

        ~StaComCrossThreadInvoker()
        {
            // do nothing - cannot dispose in finalizer thread because of creation thread affinity
        }

        /// <summary>
        /// Override to provide custom logic when retry happens. Can be used to show some notification to user.
        /// </summary>
        /// <param name="elpasedTotalWaitTimeInMilliseconds">Total time user already waited call to finish in milliseconds</param>
        /// <returns></returns>
        protected virtual bool ShouldRetry(uint elapsedTotalWaitTimeInMilliseconds)
        {
            return true;
        }

        int IMessageFilter.HandleInComingCall(uint dwCallType, IntPtr htaskCaller, uint dwTickCount, INTERFACEINFO[] lpInterfaceInfo)
        {
            return 1;
        }

        int IMessageFilter.RetryRejectedCall(IntPtr htaskCallee, uint dwTickCount, uint dwRejectType)
        {
			if (dwTickCount > _maximumTotalWaitTimeInMilliseconds)
			{
			    return CANCEL;
			}
            if (ShouldRetry(dwTickCount))
            {
                return DEFAULT_RETRY_TIMEOUT;    
            }
            return CANCEL;
        }

        int IMessageFilter.MessagePending(IntPtr htaskCallee, uint dwTickCount, uint dwPendingType)
        {
            return 1;
        }


    }
}
