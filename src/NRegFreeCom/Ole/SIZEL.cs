using System.Runtime.InteropServices;

namespace NRegFreeCom.Ole
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class SIZEL
    {
        public int cx;
        public int cy;
        public SIZEL() { }
        public SIZEL(int cx, int cy) { this.cx = cx; this.cy = cy; }
        public SIZEL(SIZEL o) { this.cx = o.cx; this.cy = o.cy; }
    }
}