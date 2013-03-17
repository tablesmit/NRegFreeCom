using System;
using System.Collections.Generic;

namespace NRegFreeCom
{
    //https://sites.google.com/site/jozsefbekes/Home/windows-programming/dotnet-registering-an-object-to-the-running-object-table-from-a-non-com-project
    //https://sites.google.com/site/jozsefbekes/Home/windows-programming/registering-an-object-to-the-running-object-table-from-a-non-com-project
    public class RotWrapper : IDisposable
    {


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
        List<ObjectInRot> _RegisteredObjects = new List<ObjectInRot>();


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
