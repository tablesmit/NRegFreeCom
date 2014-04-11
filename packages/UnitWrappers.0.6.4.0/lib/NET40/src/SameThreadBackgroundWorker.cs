using System;
using System.ComponentModel;
using UnitWrappers.System.ComponentModel;

namespace UnitWrappers
{
    public class SameThreadBackgroundWorker :  IBackgroundWorker
    {
        private Action _doWork;
        private Action _runWorkerCompleted;
        private bool _isInProcess;

        public Action DoWork
        {
            set { _doWork = value; }
        }

        public Action RunWorkerCompleted
        {
            set { _runWorkerCompleted = value; }
        }

        event RunWorkerCompletedEventHandler IBackgroundWorker.RunWorkerCompleted
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public void CancelAsync()
        {
            CancellationPending = true;
        }

        public void ReportProgress(int percentProgress)
        {
            throw new NotImplementedException();
        }

        public void ReportProgress(int percentProgress, object userState)
        {
            throw new NotImplementedException();
        }

        public void RunWorkerAsync()
        {
            RunWorker();
        }

        public void RunWorkerAsync(object argument)
        {
            throw new NotImplementedException();
        }

        private void RunWorker()
        {
            _isInProcess = true;
            if (_doWork != null)
            {
                _doWork();
            }
            if (_runWorkerCompleted != null)
            {
                _runWorkerCompleted();
            }
            _isInProcess = false;
        }

        public bool CancellationPending { get; private set; }
        public bool IsBusy { get; private set; }
        public bool WorkerReportsProgress { get; set; }
        public bool WorkerSupportsCancellation { get; set; }
        event DoWorkEventHandler IBackgroundWorker.DoWork
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public event ProgressChangedEventHandler ProgressChanged;

        public bool IsInProcess
        {
            get { return _isInProcess; }
        }

        public void Abort()
        {
            CancellationPending = true;
        }

        public void Dispose()
        {
            
        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;
    }
}
