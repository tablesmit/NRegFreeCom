using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{
    [Guid(SimpleObjectId.CustomEventsId)]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICustomSimpleObjectEvents
    {

        [DispId(1)]
        void EmptyEvent();

    }
}