using System.Runtime.InteropServices;

namespace NRegFreeCom.Interop.ComTypes
{
    [ComImport, Guid("00000104-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumOLEVERB
    {
        [PreserveSig]
        int Next([MarshalAs(UnmanagedType.U4)] int celt, [Out] OLEVERB rgelt, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);
        [PreserveSig]
        int Skip([In, MarshalAs(UnmanagedType.U4)] int celt);
        void Reset();
        void Clone(out IEnumOLEVERB ppenum);
    }
}