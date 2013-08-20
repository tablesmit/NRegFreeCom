using System;
using System.Runtime.InteropServices;
using System.Security;

namespace NRegFreeCom.Interop.ComTypes
{

    ///<seealso href="http://msdn.microsoft.com/en-us/library/cc678965.aspx"/>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    [Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
    [SecurityCritical(SecurityCriticalScope.Everything)]
    [ComImport]
    public interface IServiceProvider
    {
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object QueryService(ref Guid service, ref Guid riid);
    }
}