using System;
using System.Runtime.InteropServices;
using System.Threading;
using NRegFreeCom.Interop;

namespace NRegFreeCom.Threading
{
    public static class WindowsWaitHandleExtensions
    {
        const UInt32 INFINITE = 0xFFFFFFFF;
        const UInt32 WAIT_ABANDONED = 0x00000080;
        const UInt32 WAIT_OBJECT_0 = 0x00000000;
        const UInt32 WAIT_TIMEOUT = 0x00000102;
        const UInt32 WAIT_FAILED = INFINITE;

        /// <summary>
        /// Waits preventing an I/O completion routine or an APC for execution by the waiting thread (unlike default `alertable`  .NET wait). 
        /// E.g. prevents STA message pump in background.
        /// </summary>
        public static bool WaitOneNonAlertable(this WaitHandle current){
        	return WaitOneNonAlertable(current,INFINITE);
        }
        
                
        /// <summary>
        /// Waits preventing an I/O completion routine or an APC for execution by the waiting thread (unlike default `alertable`  .NET wait). 
        /// E.g. prevents STA message pump in background.
        /// </summary>
        /// <returns></returns>
        /// <seealso cref="http://stackoverflow.com/questions/8431221/why-did-entering-a-lock-on-a-ui-thread-trigger-an-onpaint-event">
        /// Why did entering a lock on a UI thread trigger an OnPaint event?
        /// </seealso>
        public static bool WaitOneNonAlertable(this WaitHandle current, int millisecondsTimeout){
        	if (millisecondsTimeout < -1)
                throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, "Bad wait timeout");
        	return WaitOneNonAlertable(current,(UInt32)millisecondsTimeout);
        }

        private static bool WaitOneNonAlertable(this WaitHandle current, UInt32 millisecondsTimeout)
        {
            uint ret = NativeMethods.WaitForSingleObject(current.SafeWaitHandle, millisecondsTimeout);
            switch (ret)
            {
                case WAIT_OBJECT_0:
                    return true;
                case WAIT_TIMEOUT:
                    return false;
                case WAIT_ABANDONED:
                    throw new AbandonedMutexException();
                case WAIT_FAILED:
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                default:
                    return false;
            }
        }
    }
}