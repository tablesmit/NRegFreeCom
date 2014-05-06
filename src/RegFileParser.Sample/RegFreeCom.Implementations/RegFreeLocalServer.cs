using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using RegFreeCom.Interfaces;

namespace RegFreeCom.Implementations
{
    [ComVisible(true)]
    [Guid(RegFreeComIds.CLSID)]
    [ComDefaultInterface(typeof(IRegFreeCom))]
    public class RegFreeLocalServer : IRegFreeCom
    {



        public string Info
        {
            get
            {
                return
                    string.Format("Type:{0}, ProcessName:{1}, ProcessId:{2},ThreadId:{3}",
                   this.GetType().FullName, Process.GetCurrentProcess().ProcessName, Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId);
            }
        }

        public void Request(string hello)
        {
            Console.WriteLine("Request was done with:{0}", hello);
        }

        public void Ping()
        {
            Console.WriteLine("Ping");
        }

        public int Answer()
        {
            return 42;
        }
    }
}
