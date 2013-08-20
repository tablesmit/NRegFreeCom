using System.Runtime.InteropServices;

namespace NRegFreeCom.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class OLEVERB
    {
        public int lVerb;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszVerbName;
        [MarshalAs(UnmanagedType.U4)]
        public int fuFlags;
        [MarshalAs(UnmanagedType.U4)]
        public int grfAttribs;
        public OLEVERB() { }
    }
}