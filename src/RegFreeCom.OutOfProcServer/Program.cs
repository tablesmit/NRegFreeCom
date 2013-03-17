
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NRegFreeCom;
using RegFreeCom;
using RegFreeCom.Interfaces;


namespace CSExeCOMServer
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
                using (RotWrapper rw = new RotWrapper())
                {
                    IRegFreeComRotClass rc = new RegFreeComRotClass();
                    if (rw.RegisterObject(rc, typeof(IRegFreeComRotClass).FullName) != 0)
                    {
                        Console.WriteLine("RegisterObject MyDbgToolObject failed");
                        return ;
                    }

                    // MessagePump is going while the msgbox is displayed. This thread
                    // needs to remain active as long as the object is in the ROT because
                    // this thread will server the requests (function calls).
                    Console.WriteLine("Object is in ROT");
                    // msgbox is closed, object gets revoked from rot as using block exits.
                    // Thread (even process) can also exit.
                    NRegFreeCom.MSG msg;
                    while (NRegFreeCom.NativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0))
                    {
                        NRegFreeCom.NativeMethods.TranslateMessage(ref msg);
                        NRegFreeCom.NativeMethods.DispatchMessage(ref msg);
                    }
                }

                });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            //return 0;
            //  Debugger.Launch();
            // Run the out-of-process COM server
            SimpleObjectServer.Instance.Run();
        }
    }
}

