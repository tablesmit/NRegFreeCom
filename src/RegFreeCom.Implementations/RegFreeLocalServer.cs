using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using RegFreeCom.Interfaces;

namespace RegFreeCom
{
    [ComVisible(true)]
    [Guid("9C21B7EB-7E27-4405-BCE0-62B338DF83BB")]
    [ComDefaultInterface(typeof(IRegFreeCom))]
    public class RegFreeLocalServer : IRegFreeCom
    {
        public byte[] Do(byte[] data)
        {
            return new byte[]{1};
        }
        public string ProcName { get { return Process.GetCurrentProcess().ProcessName + " " + Process.GetCurrentProcess().Id + " " + Thread.CurrentThread.ManagedThreadId; } }
        public string GetString(int number)
        {
            return ProcName + " "+number;
        }
    }
}
