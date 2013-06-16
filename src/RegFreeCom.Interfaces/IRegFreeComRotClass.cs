using System.Runtime.InteropServices;

namespace RegFreeCom.Interfaces
{
    [Guid(RotIds.IID)]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IRegFreeComRotClass
    {

        [ComVisible(true)]
        string Info { get; }

        [ComVisible(true)]
        void Request(string hello);

        [ComVisible(true)]
        void Ping();

        [ComVisible(true)]
        int Answer();

        ISimpleObject Create();

     

    }
}