using System;
using System.Globalization;
using System.Threading;
using UnitWrappers.System.Threading;

namespace UnitWrappers
{
    public abstract class ThreadWrapBase : IThread
    {
        protected Thread _thread;

        public CultureInfo CurrentCulture
        {
            get { return _thread.CurrentCulture; }
            set { _thread.CurrentCulture = value; }
        }

        public int ManagedThreadId { get { return _thread.ManagedThreadId; } }
        public CultureInfo CurrentUICulture { get { return _thread.CurrentUICulture; } set { _thread.CurrentUICulture = value; } }
        public ExecutionContext ExecutionContext { get { return _thread.ExecutionContext; } }
        public string Name { get { return _thread.Name; } set { _thread.Name = value; } }
        public bool IsAlive
        {
            get { return _thread.IsAlive; }
        }

        public bool IsThreadPoolThread { get { return _thread.IsThreadPoolThread; } }
        public ThreadState ThreadState { get { return _thread.ThreadState; } }
        public bool IsBackground { get { return _thread.IsBackground; } set { _thread.IsBackground = value; } }
        public ThreadPriority Priority { get { return _thread.Priority; } set { _thread.Priority = value; } }

        public ApartmentState GetApartmentState()
        {
            return _thread.GetApartmentState();
        }

        public void SetApartmentState(ApartmentState state)
        {
            _thread.SetApartmentState(state);
        }

        public bool TrySetApartmentState(ApartmentState state)
        {
            return _thread.TrySetApartmentState(state);
        }

        public void Abort() { }
        public void Abort(object stateInfo) { }
        public abstract void Start();
        public abstract void Start(object parameter);
        public void Join() { }
        public bool Join(int millisecondsTimeout)
        {
            return true;
        }
        public bool Join(TimeSpan timeout)
        {
            return true;
        }
        public void Interrupt() { }

    }
}