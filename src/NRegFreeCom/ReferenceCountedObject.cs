using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace NRegFreeCom
{
    /// <summary>
    /// Reference counted object base or delegation.
    /// </summary>
    [ComVisible(false)]
    public class ReferenceCountedObject
    {
        // The lock count (the number of active COM objects) in the server
        private int _nLockCnt = 0;

        public event EventHandler NoReferenceEvent;

        public ReferenceCountedObject()
        {
            //
            // Initialize member variables.
            // 
            // Records the count of the active COM objects in the server. 
            // When _nLockCnt drops to zero, the server can be shut down.
            _nLockCnt = 0;
            Lock();
        }

        /// <summary>
        ///  Increment the lock count of objects in the COM server.
        /// </summary>
        /// <returns>The new lock count after the increment</returns>
        /// <remarks>The method is thread-safe.</remarks>
        public int Lock()
        {
            return Interlocked.Increment(ref _nLockCnt);
        }

        /// <summary>
        /// Get the current lock count.
        /// </summary>
        /// <returns></returns>
        public int LockCount
        {
            get
            {
                return _nLockCnt;
            }

        }

        /// <summary>
        // Decrement the lock count of objects in the COM server.
        /// Decrease the lock count. When the lock count drops to zero, post 
        /// the WM_QUIT message to the message loop in the main thread to 
        /// shut down the COM server.
        /// </summary>
        /// <returns>The new lock count after the increment</returns>
        public int Unlock()
        {
            int nRet = Interlocked.Decrement(ref _nLockCnt);

            // If lock drops to zero, attempt to terminate the server.
            if (nRet == 0)
            {
                var handle = NoReferenceEvent;
                if (handle != null)
                    NoReferenceEvent(this, EventArgs.Empty);

            }
            return nRet;
        }

        ~ReferenceCountedObject()
        {

            Unlock();
        }
    }
}