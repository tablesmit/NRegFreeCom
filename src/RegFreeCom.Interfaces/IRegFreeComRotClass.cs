using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{
    public class Rotguid
    {
        public const string IID = "A699BC89-96CF-476B-86F0-2CB63EB8C4E4";
    }
    [Guid(Rotguid.IID)]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IRegFreeComRotClass
    {

        float FloatProperty { get; set; }

        string ProcName { get; }

        ISimpleObject Create();

        byte[] Execute(byte[] request);

    }
}