using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{
    [ComVisible(true)]
    [Guid("144E6335-0F09-4218-8182-A650C0B63601")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMyCoolClass
    {
        string MyValue { [DispId(1)]get; }

        [DispId(3)]
        int GetMyValue2(out string value);
    }
}