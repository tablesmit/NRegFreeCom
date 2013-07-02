
using System;
using System.Runtime.InteropServices;
using System.Threading;


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

            var regServices = new RegistrationServices();
            
            int cookie = regServices.RegisterTypeForComClients(
                typeof(RegFreeCom.Implementations.SimpleObject),
                RegistrationClassContext.LocalServer | RegistrationClassContext.RemoteServer,
                RegistrationConnectionType.MultipleUse);

            Console.WriteLine("Ready"); Console.ReadKey();

            regServices.UnregisterTypeForComClients(cookie);

            var t2 = new Thread(() =>
{
    SimpleObjectServer.Instance.Run();

});
            t2.SetApartmentState(ApartmentState.STA);
            t2.Start();
            t2.Join();


        }
    }
}

