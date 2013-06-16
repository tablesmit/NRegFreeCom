using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{

    [Guid(SimpleObjectId.InterfaceId)]
    [ComVisible(true)]
    public interface ISimpleObject
    {
        [DispId(1)]
        float FloatProperty { get; set; }
        [DispId(3)]
        string HelloWorld();

        [DispId(5)]
        void GetProcessThreadID(out uint processId, out uint threadId);

        [DispId(6)]
        string Info { get; }

        [DispId(7)]
        void RaisePassStruct();


        
        // one of ways to define callbacks usable from C++
        ISimpleObjectEvents Callbacks { [DispId(9)] get; [DispId(11)] set; }

        [DispId(13)]
        void RaisePassClass();
        [DispId(15)]
        void RaisePassString();

        [DispId(17)]
        void RaiseEnsureGCIsNotObstacle();


         [DispId(19)]
        void RaiseEmptyEvent();
        

    }
}