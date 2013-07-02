using System;
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

        private Thread _thread;
        private readonly uint _threadId;
        private bool _hasShutdownFinished;
        private static ManualResetEvent _running;
        private static IntPtr _messageDispatcherWindow;
        private Delegate _currentMethod;
        private object[] _currentArgs;
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
            _threadId = threadId;
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
            get { return _running.WaitOne(0); }
        }

        private const string DISPATCHER_NAME_TEMPLATE = "NRegFreeCom Dispatcher Window {0}";
        const string DISPATCHER_CLASS_NAME_TEMPLATE = "NRegFreeCom Dispatcher Window Class {0}";
        private const int CW_USEDEFAULT = unchecked((int)0x80000000);

        public static void Run()
        {
            _running = new ManualResetEvent(false);
            var wc = new WNDCLASS();
            IntPtr hInstance = Process.GetCurrentProcess().Handle;
            CurrentDispatcher._hInstance = hInstance;
            wc.lpfnWndProc = WindowProc;
            wc.hInstance = hInstance;
            wc.lpszClassName = string.Format(DISPATCHER_CLASS_NAME_TEMPLATE, CurrentDispatcher._threadId);
            CurrentDispatcher._atom = NativeMethods.RegisterClass(ref wc);
            if (CurrentDispatcher._atom == 0)
            {
                throw new Win32Exception("Failed to register window");
            }
            _messageDispatcherWindow = NativeMethods.CreateWindowEx(
        0,
          new IntPtr((int)CurrentDispatcher._atom),// fixes "Cannot find window class." when uses string
        string.Format(DISPATCHER_NAME_TEMPLATE, CurrentDispatcher._threadId),
       WindowStyles.WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
        IntPtr.Zero,
        IntPtr.Zero,
        hInstance,
        IntPtr.Zero);
            if (_messageDispatcherWindow == IntPtr.Zero)
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
            _running.Set();
        }

        private static IntPtr WindowProc(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam)
        {
            if (msg == hookMessage && wparam == new IntPtr(hookMessageDiffl) && lparam == new IntPtr(hookMessageDiffw))
            {
                CurrentDispatcher._currentMethod.DynamicInvoke(CurrentDispatcher._currentArgs);
                var invoked = CurrentDispatcher._invoked;
                _currentDispatcher = null;
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
        }

        public void Invoke(Delegate method, params object[] args)
        {
            _currentMethod = method;
            _currentArgs = args;
            _invoked = new AutoResetEvent(false);
            NativeMethods.PostMessage(_messageDispatcherWindow, hookMessage, new IntPtr(hookMessageDiffl), new IntPtr(hookMessageDiffw));
            _invoked.WaitOne();
        }
    }
}