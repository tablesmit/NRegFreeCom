using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{
    [ComVisible(true)]
    [Guid("38CEC663-83E6-4BB6-83E2-DD1CE6275E49")]
    public struct MyCoolStuct
    {
        public int _Val;
        //needs directly marshal as COM string, because by default C string used
        [System.Runtime.InteropServices.MarshalAs(UnmanagedType.BStr)]
        public string _Val2;
    }
}