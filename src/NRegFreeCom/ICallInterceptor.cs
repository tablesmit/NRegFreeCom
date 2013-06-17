using System.Runtime.InteropServices;

namespace NRegFreeCom
{
    [ComImport, ComConversionLoss, InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     Guid("60C7CA75-896D-11d2-B8B6-00C04FB9618A")]
    public interface ICallInterceptor 
    {
        int GetRegisteredSink(
            [Out] out ICallFrameEvents ppsink
            );

        int RegisterSink(
            [In]  ICallFrameEvents psink
            );
    }
}