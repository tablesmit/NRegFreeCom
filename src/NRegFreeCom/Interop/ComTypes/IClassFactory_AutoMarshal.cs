using System;
using System.Runtime.InteropServices;

namespace NRegFreeCom.Interop.ComTypes
{
    [Guid(WELL_KNOWN_IIDS.IID_IClassFactory)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface IClassFactory_AutoMarshal
    {
        void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
        void LockServer(bool fLock);
    }
}