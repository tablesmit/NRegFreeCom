using System;

namespace NRegFreeCom
{
    public static class Constants
    {
        /// <summary>
        /// Interface Id of IClassFactory
        /// </summary>
        public const string IID_IClassFactory =
            "00000001-0000-0000-C000-000000000046";

        public static IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

        public const UInt32 WM_CLOSE = 0x0010;
        public const UInt32 WM_QUIT = 0x0012;
    }
}
