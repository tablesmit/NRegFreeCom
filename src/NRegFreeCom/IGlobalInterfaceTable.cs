using System;
using System.Runtime.InteropServices;

namespace NRegFreeCom
{
    [
        ComImport(),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid("00000146-0000-0000-C000-000000000046")
    ]
    public interface IGlobalInterfaceTable
    {
        int RegisterInterfaceInGlobal(
            [MarshalAs(UnmanagedType.IUnknown)] object pUnk,
            [In()] ref Guid riid);

        void RevokeInterfaceFromGlobal(
            int dwCookie);
        [return: MarshalAs(UnmanagedType.IUnknown)]

        object GetInterfaceFromGlobal(
            int dwCookie,
            [In()] ref Guid riid);



    }
}