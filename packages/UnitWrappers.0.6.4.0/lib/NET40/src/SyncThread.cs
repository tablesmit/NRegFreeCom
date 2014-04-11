using System.Threading;

namespace UnitWrappers
{
    public class SyncThread : ThreadWrapBase
    {
        public SyncThread(ParameterizedThreadStart start)
        {
            _thread = new Thread(start);
        }

        public SyncThread(ThreadStart start)
        {
            _thread = new Thread(start);
        }

        public SyncThread(Thread thread)
        {
            _thread = thread;
        }

        public override void Start()
        {
            _thread.Start();
            _thread.Join();
        }

        public override void Start(object parameter)
        {
            _thread.Start(parameter);
            _thread.Join();
        }
    }
}