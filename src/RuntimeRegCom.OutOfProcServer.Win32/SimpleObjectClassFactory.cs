using System;
using System.Runtime.InteropServices;
using NRegFreeCom;
using RegFreeCom.Interfaces;

namespace RuntimeRegCom.OutOfProcServer.Win32
{
    

    [ComVisible(true)]
    [Guid(SimpleObjectId.Factory)]
    public class SimpleObjectClassFactory : IClassFactory
    {

      public event EventHandler NoReferenceEvent;

        public int CreateInstance(IntPtr pUnkOuter, ref Guid riid, 
                                  out IntPtr ppvObject)
        {
            ppvObject = IntPtr.Zero;

            if (pUnkOuter != IntPtr.Zero)
            {
                // The pUnkOuter parameter was non-NULL and the object does 
                // not support aggregation.
                Marshal.ThrowExceptionForHR(NativeMethods.CLASS_E_NOAGGREGATION);
            }

            if (riid == new Guid(SimpleObjectId.ClassId) ||
                riid == new Guid(WELL_KNOWN_IIDS.IID_IDispatch) ||
                riid == new Guid(WELL_KNOWN_IIDS.IID_IUnknown) ||
                   riid == new Guid(SimpleObjectId.InterfaceId))
            {
                // Create the instance of the .NET object
                var instance = new SimpleObject();
                instance.NoReferenceEvent += new EventHandler(instance_NoReferenceEvent);
                ppvObject = Marshal.GetComInterfaceForObject(
                  instance, typeof(ISimpleObject));
            }
            else
            {
                // The object that ppvObject points to does not support the 
                // interface identified by riid.
                Marshal.ThrowExceptionForHR(NativeMethods.E_NOINTERFACE);
            }

            return 0;   // S_OK
        }

        void instance_NoReferenceEvent(object sender, EventArgs e)
        {
            var handle = NoReferenceEvent;
            if (handle != null)
            {
                handle(sender, e);
            }
        }

        public int LockServer(bool fLock)
        {
            return 0;   // S_OK
        }
    }
}