using System;
using System.Threading;

namespace NRegFreeCom
{
    public interface IDispatcher
    {
        Thread Thread { get; }
        bool HasShutdownFinished { get; }
        void InvokeShutdown();
        void Invoke(Delegate method,params object[] args);
    }
}