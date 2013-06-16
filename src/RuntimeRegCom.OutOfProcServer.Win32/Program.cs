
using System;

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

