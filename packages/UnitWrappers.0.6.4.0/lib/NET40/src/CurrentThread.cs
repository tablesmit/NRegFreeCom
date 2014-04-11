using System.Threading;

namespace UnitWrappers
{

    public class CurrentThread : ThreadWrapBase
    {
        private ParameterizedThreadStart _parameterizedThreadStart;
        private ThreadStart _threadStart;
        private Thread _thread;

        public CurrentThread(ParameterizedThreadStart start)
        {
            _thread = Thread.CurrentThread;
            _parameterizedThreadStart = start;
        }

        public CurrentThread(ThreadStart start)
        {
            _thread = Thread.CurrentThread;
            _threadStart = start;
        }

        public override void Start()
        {
            _threadStart.Invoke();
        }

        public override void Start(object parameter)
        {
            _parameterizedThreadStart.Invoke(parameter);
        }




    }
}