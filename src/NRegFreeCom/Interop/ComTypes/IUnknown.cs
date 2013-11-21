using System;
using System.Runtime.InteropServices;

namespace NRegFreeCom.Interop.ComTypes
{
    [ComVisible(false)]
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(WELL_KNOWN_IIDS.IID_IUnknown)]
    public interface IUnknown
    {
        IntPtr QueryInterface(ref Guid riid);

        [PreserveSig]
        UInt32 AddRef();

        [PreserveSig]
        UInt32 Release();
    }
}