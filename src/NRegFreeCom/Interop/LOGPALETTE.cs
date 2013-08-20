using System.Runtime.InteropServices;

namespace NRegFreeCom.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class LOGPALETTE
    {
        [MarshalAs(UnmanagedType.U2)]
        public short palVersion;
        [MarshalAs(UnmanagedType.U2)]
        public short palNumEntries;
        public LOGPALETTE() { }
    }
}