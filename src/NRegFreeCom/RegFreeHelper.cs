using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace NRegFreeCom
{
    public class RegFreeHelper
    {

   


        public static void PrintRot()
        {
            IRunningObjectTable rot;
            IEnumMoniker enumMoniker;
            int retVal = NativeMethods.GetRunningObjectTable(0, out rot);

            if (retVal == 0)
            {

                rot.EnumRunning(out enumMoniker);

                IntPtr fetched = IntPtr.Zero;
                IMoniker[] moniker = new IMoniker[1];

                var str = File.Create(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CsExeComServer.log"));


                var textWriter =
                    new StreamWriter(str);

                while (enumMoniker.Next(1, moniker, fetched) == 0)
                {
                    IBindCtx bindCtx;
                    NativeMethods.CreateBindCtx(0, out bindCtx);
                    string displayName;
                    moniker[0].GetDisplayName(bindCtx, null, out displayName);
                   var running =  moniker[0].IsRunning(bindCtx,null,null);
                    textWriter.WriteLine("Display Name: {0}; Running:{1}", displayName,running);
                }
                textWriter.Flush();
                str.Dispose();
            }
        }


    }
}
