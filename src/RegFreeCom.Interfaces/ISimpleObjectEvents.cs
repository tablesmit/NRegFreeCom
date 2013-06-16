using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{


    [Guid(SimpleObjectId.EventsId)]
    [ ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ISimpleObjectEvents
    {

        [DispId(1)]
        void FloatPropertyChanging(float NewValue, ref bool Cancel);

        [DispId(3)]
        void PassStuct(MyCoolStuct val);

        [DispId(5)]
        void PassClass(IMyCoolClass obj);
        [DispId(7)]
        void PassString(string str);

              [DispId(9)]
        void EnsureGCIsNotObstacle();

         [DispId(11)]
        void   SimpleEmptyEvent();
    }


    [Guid(SimpleObjectId.CustomEventsId)]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICustomSimpleObjectEvents
    {

        [DispId(1)]
        void EmptyEvent();

    }
}