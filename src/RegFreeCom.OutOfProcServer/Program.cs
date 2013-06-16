using System;
using System.Threading;
using NRegFreeCom;
using RegFreeCom.Implementations;
using RegFreeCom.Interfaces;

namespace RegFreeCom.OutOfProcServer
{

    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var t = new Thread(() =>
            {
                using (var rw = new RunningObjectTable())
                {
                    IRegFreeComRotClass rc = new RegFreeComRotClass();
                    if (rw.RegisterObject(rc, typeof(IRegFreeComRotClass).FullName) != 0)
                    {
                        Console.WriteLine("RegisterObject MyDbgToolObject failed");
                        return;
                    }

                    // This thread
                    // needs to remain active as long as the object is in the ROT because
                    // this thread will server the requests (function calls).
                    Console.WriteLine("Object is in ROT");

                    NRegFreeCom.MSG msg;
                    while (NRegFreeCom.NativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0))
                    {
                        // if wm_quit received, object gets revoked from rot as using block exits.
                        // Thread (even process) can also exit.
                        NRegFreeCom.NativeMethods.TranslateMessage(ref msg);
                        NRegFreeCom.NativeMethods.DispatchMessage(ref msg);
                    }
                }

            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();


        }
    }
}

