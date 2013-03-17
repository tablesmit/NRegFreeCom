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

        public RegFreeLocalServer(){}
        public byte[] Do(byte[] data)
        {
            return new byte[]{1};
        }
        public string ProcName { get { return Process.GetCurrentProcess().ProcessName + " " + Process.GetCurrentProcess().Id + " " + Thread.CurrentThread.ManagedThreadId; } }
        public string GetString(int number)
        {
            return ProcName + " "+ number;
        }
    }
}
