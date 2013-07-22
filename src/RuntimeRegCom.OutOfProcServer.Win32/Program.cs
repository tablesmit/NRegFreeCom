using System;
using System.Threading;
using NRegFreeCom;

namespace RuntimeRegCom.OutOfProcServer.Win32
{

    class Program
    {
        private static Dispatcher _dispatcher;
        private static SimpleObjectServer _server;

        // The timer to trigger GC every 5 seconds
        private  static  Timer _gcTimer;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            //var regServices = new RegistrationServices();
            
            //int cookie = regServices.RegisterTypeForComClients(
            //    typeof(RegFreeCom.Implementations.SimpleObject),
            //    RegistrationClassContext.LocalServer | RegistrationClassContext.RemoteServer,
            //    RegistrationConnectionType.MultipleUse);

            //Console.WriteLine("Ready"); Console.ReadKey();

            //regServices.UnregisterTypeForComClients(cookie);


            var t2 = new Thread(() =>
                {
                 _dispatcher =   Dispatcher.CurrentDispatcher;
                    Dispatcher.Run();
                });

            t2.SetApartmentState(ApartmentState.STA);
            t2.Start();
            var wait = new SpinWait();
            while (_dispatcher == null)
               wait.SpinOnce();
            _dispatcher.Invoke(new Action(() =>
                {
                    _server = new SimpleObjectServer(_dispatcher);
                    _server.Run();
                    Console.WriteLine("Server started");
                }));

            // Start the GC timer to trigger GC every 5 seconds.
            // TO see to make .NET GC server, and we stop COM dispatch and stop process
            _gcTimer = new Timer(new TimerCallback(GarbageCollect), null,
                5000, 5000);

            t2.Join();


        }

        private static void GarbageCollect(object state)
        {
            GC.Collect();
        }
    }
}

