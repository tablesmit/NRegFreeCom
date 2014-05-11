using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NRegFreeCom.Interop;

namespace NRegFreeCom
{
    /// <summary>
    /// Raw dispatcher with good access to underlying Windows Message Pump if needed.
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

        private class Invocation
        {
            public Delegate Function;
            public object[] Args;

            public Invocation(Delegate method, object[] args)
            {
                Function = method;
                Args = args;
            }

            protected bool Equals(Invocation other)
            {
                return Equals(Args, other.Args) && Equals(Function, other.Function);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Invocation) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Args != null ? Args.GetHashCode() : 0)*397) ^ (Function != null ? Function.GetHashCode() : 0);
                }
            }
        }

        private Queue<Invocation> _invokes = new Queue<Invocation>();

        private AutoResetEvent _invoked;
        private ushort _atom;
        private IntPtr _hInstance;

        // used to provide WPF dispatcher like behavior
        const uint hookMessage = (uint)WM.USER + 3662;
        const uint hookMessageDiffw = 521;
        const uint hookMessageDiffl = 456;

        private Dispatcher(Thread thread, uint threadId)
        {
            _thread = thread;
            // Records the ID of the thread that runs the message loop so that 
            // that known where to post the WM_QUIT message to exit the 
            // message loop.
            _threadId = threadId;
            _running = new ManualResetEvent(false);
        }

        public Thread Thread
        {
            get { return _thread; }
        }

        public IntPtr Handle
        {
            get { return _messageDispatcherWindow; }
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
            MSG msg;
            while (NativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                // if wm_quit received, object gets revoked from rot as using block exits.
                // Thread (even process) can also exit.
                NativeMethods.TranslateMessage(ref msg);
                NativeMethods.DispatchMessage(ref msg);
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
                invoke.Function.DynamicInvoke(invoke.Args);
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
            _invokes.Enqueue(new Invocation(method, args));

            _invoked = new AutoResetEvent(false);
            NativeMethods.PostMessage(_messageDispatcherWindow, hookMessage, new IntPtr(hookMessageDiffl), new IntPtr(hookMessageDiffw));
            _invoked.WaitOne();
        }
    }
}