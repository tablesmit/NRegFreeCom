using System;

namespace NRegFreeCom.Interop
{
    public static class DEF
    {

        ///<seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms682583.aspx"/>
        public delegate bool DllMain(IntPtr hModule, int reason, IntPtr lpReserved);
    }
}
