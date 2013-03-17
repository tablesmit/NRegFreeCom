using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{

    [Guid(SimpleObjectId.InterfaceId)]
    [ComVisible(true)]
    public interface ISimpleObject
    {
        float FloatProperty { get; set; }

        string HelloWorld();

        void GetProcessThreadID(out uint processId, out uint threadId);
        string ProcName { get; }
    }
}