using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NRegFreeCom.Interop.ComTypes
{
    [ComImport, ComConversionLoss, InterfaceType((short)1),
    Guid("00000016-0000-0000-C000-000000000046")]
    public interface IMessageFilter
    {
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall,
            MethodCodeType = MethodCodeType.Runtime)]
        int HandleInComingCall([In] uint dwCallType, [In] IntPtr htaskCaller,
            [In] uint dwTickCount,
            [In, MarshalAs(UnmanagedType.LPArray)] INTERFACEINFO[]
            lpInterfaceInfo);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall,
            MethodCodeType = MethodCodeType.Runtime)]
        int RetryRejectedCall([In] IntPtr htaskCallee, [In] uint dwTickCount,
            [In] uint dwRejectType);

        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall,
            MethodCodeType = MethodCodeType.Runtime)]
        int MessagePending([In] IntPtr htaskCallee, [In] uint dwTickCount,
            [In] uint dwPendingType);
    }


}
