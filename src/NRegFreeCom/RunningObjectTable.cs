using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace NRegFreeCom
{
      

    ///<summary>
    /// Allows manipulations with Running Object Table like add and removing objects, enumerating registered. 
    /// </summary>
    ///<seealso href="https://sites.google.com/site/jozsefbekes/Home/windows-programming/dotnet-registering-an-object-to-the-running-object-table-from-a-non-com-project"/>
    ///<seealso href="https://sites.google.com/site/jozsefbekes/Home/windows-programming/registering-an-object-to-the-running-object-table-from-a-non-com-project"/>
    public class RunningObjectTable : IDisposable
    {
        List<ObjectInRot> _RegisteredObjects = new List<ObjectInRot>();

        public  void PrintRot()
        {
            IRunningObjectTable rot;
            IEnumMoniker enumMoniker;
            int retVal = NativeMethods.GetRunningObjectTable(0, out rot);

            if (retVal == 0)
            {

                rot.EnumRunning(out enumMoniker);

                IntPtr fetched = IntPtr.Zero;
                IMoniker[] moniker = new IMoniker[1];
                //TODO: returen MonikerInfo[] instead of direcly writing to file
                var str = File.Create(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "CsExeComServer.log"));


                var textWriter =
                    new StreamWriter(str);

                while (enumMoniker.Next(1, moniker, fetched) == 0)
                {
                    IBindCtx bindCtx;
                    NativeMethods.CreateBindCtx(0, out bindCtx);
                    string displayName;
                    moniker[0].GetDisplayName(bindCtx, null, out displayName);
                    var running = moniker[0].IsRunning(bindCtx, null, null);
                    textWriter.WriteLine("Display Name: {0}; Running:{1}", displayName, running);
                }
                textWriter.Flush();
                str.Dispose();
            }
        }

        public int RegisterObject(object obj, string stringId)
        {
            int regId = -1;

            System.Runtime.InteropServices.ComTypes.IRunningObjectTable pROT = null;
            System.Runtime.InteropServices.ComTypes.IMoniker pMoniker = null;

            int hr;

            if ((hr = NativeMethods.GetRunningObjectTable((uint)0, out pROT)) != 0)
            {
                return (hr);
            }

            // File Moniker has to be used because in VBS GetObject only works with file monikers in the ROT
            if ((hr = NativeMethods.CreateFileMoniker(stringId, out pMoniker)) != 0)
            {

                return hr;
            }

            int ROTFLAGS_REGISTRATIONKEEPSALIVE = 1;
            regId = pROT.Register(ROTFLAGS_REGISTRATIONKEEPSALIVE, obj, pMoniker);

            _RegisteredObjects.Add(new ObjectInRot(obj, regId));

            return 0;
        }


        class ObjectInRot
        {
            public ObjectInRot(object obj, int regId)
            {
                this.obj = obj;
                this.regId = regId;
            }
            public object obj;
            public int regId;
        };
     

        public void Dispose()
        {
            foreach (ObjectInRot obj in _RegisteredObjects)
            {
                NativeMethods.RevokeActiveObject(obj.regId, IntPtr.Zero);
            }
            _RegisteredObjects.Clear();
        }

    }
}
