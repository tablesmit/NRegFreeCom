using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{


    [Guid(SimpleObjectId.EventsId), ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ISimpleObjectEvents
    {

        [DispId(1)]
        void FloatPropertyChanging(float NewValue, ref bool Cancel);

    }
}