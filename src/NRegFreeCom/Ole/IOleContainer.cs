using System.Runtime.InteropServices;

namespace NRegFreeCom.Ole
{
    [ComImport]
    [Guid("0000011b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleContainer
    {
        void EnumObjects(uint grfFlags, ref object IEnumUnknown);
        void LockContainer(bool fLock);
    }
}