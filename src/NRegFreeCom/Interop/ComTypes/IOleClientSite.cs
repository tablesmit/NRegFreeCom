using System.Runtime.InteropServices;

namespace NRegFreeCom.Interop.ComTypes
{
    [ComImport]
    [Guid("00000118-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleClientSite
    {
        void SaveObject();
        void GetMoniker(uint dwAssign, uint dwWhichMoniker, ref object ppmk);
        void GetContainer(ref IOleContainer ppContainer);
        void ShowObject();
        void OnShowWindow(bool fShow);
        void RequestNewObjectLayout();
    }
}