using System;
using System.Threading;

namespace NRegFreeCom
{
    /// <summary>
    /// Like <see cref="System.Windows.Threadin.Dispatcher"/> but without WPF specific stuff.
    /// </summary>
    public interface IDispatcher
    {
        Thread Thread { get; }
        bool HasShutdownFinished { get; }
        void InvokeShutdown();
        void Invoke(Delegate method,params object[] args);
    }
}