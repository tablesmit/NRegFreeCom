using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace NRegFreeCom.Ole
{
    [SuppressUnmanagedCodeSecurity]
    public static class NativeMethods
    {
        ///<summary>
        ///http://msdn.microsoft.com/en-us/library/windows/desktop/ms678485.aspx
        ///                HRESULT OleLoadPicturePath(
        ///  _In_   LPOLESTR szURLorPath,
        ///  _In_   LPUNKNOWN punkCaller,
        ///  _In_   DWORD dwReserved,
        ///  _In_   OLE_COLOR clrReserved,
        ///  _In_   REFIID riid,
        ///  _Out_  LPVOID *ppvRet
        ///);
        ///   </summary>
        [DllImport("oleaut32.dll")]
        public static extern int OleLoadPicturePath(
             string szURLorPath,
             IntPtr punkCaller,
             uint dwReserved,
             uint clrReserved,
            ref Guid riid,
             out IUnknown unknown);

    }
}
