using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace NRegFreeCom
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Threading.Dispatcher"/>
    public class Dispatcher : IDispatcher
    {
        [ThreadStatic]
        private static Dispatcher _currentDispatcher;

        private ManualResetEvent _running;
        private IntPtr _messageDispatcherWindow;
        private Thread _thread;
        private readonly uint _threadId;
        private bool _hasShutdownFinished;
        private Queue<Tuple<Delegate, object[]>> _invokes = new Queue<Tuple<Delegate, object[]>>();

        private AutoResetEvent _invoked;
        private ushort _atom;
        private IntPtr _hInstance;

        // used to provide WPF dispatcher like behaviour
        const uint hookMessage = (uint)WM.USER + 3662;
        const uint hookMessageDiffw = 521;
        const uint hookMessageDiffl = 456;

        private Dispatcher(Thread thread, uint threadId)
        {
            _thread = thread;
            // Records the ID of the thread that runs the messae loop so that 
            // that known where to post the WM_QUIT message to exit the 
            // message loop.
            _threadId = threadId;
            _running = new ManualResetEvent(false);
        }

        public Thread Thread
        {
            get { return _thread; }
        }

        public static Dispatcher CurrentDispatcher
        {
            get
            {
                if (_currentDispatcher == null)
                {
                    _currentDispatcher = new Dispatcher(Thread.CurrentThread, NativeMethods.GetCurrentThreadId());
                }
                return _currentDispatcher;
            }

        }

        public bool HasShutdownFinished
        {
            get
            {
                // block until  was ever run
                return _hasShutdownFinished;
            }
        }

        private const string DISPATCHER_NAME_TEMPLATE = "NRegFreeCom Dispatcher Window {0}";
        const string DISPATCHER_CLASS_NAME_TEMPLATE = "NRegFreeCom Dispatcher Window Class {0}";
        private const int CW_USEDEFAULT = unchecked((int)0x80000000);

        /// <summary>
        /// Runs the standard message loop. The message loop quits when it receives the WM_QUIT message.
        /// </summary>
        public static void Run()
        {
            var wc = new WNDCLASS();
            IntPtr hInstance = Process.GetCurrentProcess().Handle;
            _currentDispatcher._hInstance = hInstance;
            wc.lpfnWndProc = WindowProc;
            wc.hInstance = hInstance;
            wc.lpszClassName = string.Format(DISPATCHER_CLASS_NAME_TEMPLATE, _currentDispatcher._threadId);
            _currentDispatcher._atom = NativeMethods.RegisterClass(ref wc);
            if (_currentDispatcher._atom == 0)
            {
                throw new Win32Exception("Failed to register window");
            }
            _currentDispatcher._messageDispatcherWindow = NativeMethods.CreateWindowEx(
          0,
            new IntPtr((int)_currentDispatcher._atom),// fixes "Cannot find window class." when uses string
          string.Format(DISPATCHER_NAME_TEMPLATE, _currentDispatcher._threadId),
         WindowStyles.WS_OVERLAPPEDWINDOW,
          CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
          IntPtr.Zero,
          IntPtr.Zero,
          hInstance,
          IntPtr.Zero);
            if (_currentDispatcher._messageDispatcherWindow == IntPtr.Zero)
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new Win32Exception(lastError);
            }
            NRegFreeCom.MSG msg;
            while (NRegFreeCom.NativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                // if wm_quit received, object gets revoked from rot as using block exits.
                // Thread (even process) can also exit.
                NRegFreeCom.NativeMethods.TranslateMessage(ref msg);
                NRegFreeCom.NativeMethods.DispatchMessage(ref msg);
            }

        }

        private static IntPtr WindowProc(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam)
        {
            // dispatch cycle started, is done by default Windows messages after loop started
            if (hwnd != IntPtr.Zero && msg == (uint)WM.NCCREATE)
            {
                _currentDispatcher._messageDispatcherWindow = hwnd;
                _currentDispatcher._running.Set();
            }
               


            if (msg == hookMessage && wparam == new IntPtr(hookMessageDiffl) && lparam == new IntPtr(hookMessageDiffw))
            {
                var val = NativeMethods.GetCurrentThreadId();
                var m = Thread.CurrentThread.ManagedThreadId;
                var invoke = _currentDispatcher._invokes.Dequeue();
                invoke.Item1.DynamicInvoke(invoke.Item2);
                var invoked = _currentDispatcher._invoked;
                invoked.Set();
            }

            return NativeMethods.DefWindowProc(hwnd, msg, wparam, lparam);
        }

        public void InvokeShutdown()
        {
            NativeMethods.PostMessage(_messageDispatcherWindow, (uint)WM.QUIT, IntPtr.Zero, IntPtr.Zero);
            _running.WaitOne();
            NativeMethods.DestroyWindow(_messageDispatcherWindow);
            NativeMethods.UnregisterClass(new IntPtr((int)_atom), _hInstance);
            _hasShutdownFinished = true;
        }

        public void Invoke(Delegate method, params object[] args)
        {
            _running.WaitOne();
            _invokes.Enqueue(new Tuple<Delegate, object[]>(method, args));

            _invoked = new AutoResetEvent(false);
            NativeMethods.PostMessage(_messageDispatcherWindow, hookMessage, new IntPtr(hookMessageDiffl), new IntPtr(hookMessageDiffw));
            _invoked.WaitOne();
        }
    }
}