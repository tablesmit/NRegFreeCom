using System;
using System.Runtime.InteropServices;
using NRegFreeCom;
using RegFreeCom.Interfaces;

namespace RegFreeCom.Implementations
{
    

    [ComVisible(true)]
    [Guid(SimpleObjectId.Factory)]
    public class SimpleObjectClassFactory : IClassFactory
    {
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
                ppvObject = Marshal.GetComInterfaceForObject(
                    new SimpleObject(), typeof(ISimpleObject));
            }
            else
            {
                // The object that ppvObject points to does not support the 
                // interface identified by riid.
                Marshal.ThrowExceptionForHR(NativeMethods.E_NOINTERFACE);
            }

            return 0;   // S_OK
        }

        public int LockServer(bool fLock)
        {
            return 0;   // S_OK
        }
    }
}