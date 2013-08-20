using System.Runtime.InteropServices;

namespace NRegFreeCom.Interop.ComTypes
{
    [ComImport, ComConversionLoss, InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     Guid("FD5E0843-FC91-11d0-97D7-00C04FB9618A")]
    public interface ICallFrameEvents 
    {
        int OnCall(
            [In]  [MarshalAs(UnmanagedType.IUnknown)] object pFrame
            );
    }
}