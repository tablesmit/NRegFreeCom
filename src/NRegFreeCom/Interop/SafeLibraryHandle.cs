using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace NRegFreeCom.Interop
{
    [SuppressUnmanagedCodeSecurity]
    [System.Security.SecurityCritical]
    public sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal SafeLibraryHandle()
            : base(true)
        {
        }



        protected override bool ReleaseHandle()
        {
            return NativeMethods.FreeLibrary(this.handle);
        }
    }
}