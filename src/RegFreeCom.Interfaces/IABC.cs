using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RegFreeCom.Interfaces
{
    // [
    //  odl,
    //  uuid(641BA85E-DC0C-4AAB-AC03-4F5C1AAAD595),
    //  version(1.0),
    //  dual,
    //  oleautomation,
    //  custom(0F21F359-AB84-41E8-9A78-36D110E6D2F9, "RegFreeCom.Interfaces.IABC")    

    //]
    //interface IABC : IDispatch {
    //    [id(0x60020000)]
    //    HRESULT B();
    //    [id(0x60020001)]
    //    HRESULT C();
    //    [id(0x60020002)]
    //    HRESULT A();
    //};



    [ComVisible(true)]
    [Guid("641BA85E-DC0C-4AAB-AC03-4F5C1AAAD595")]
    public interface IABC
    {
        void B();
        void C();
        void A();
    }


    //[
    //  odl,
    //  uuid(CD9A1BA0-621A-4D2A-A6CC-7B16EB79C3D3),
    //  version(1.0),
    //  dual,
    //  oleautomation,
    //  custom(0F21F359-AB84-41E8-9A78-36D110E6D2F9, "RegFreeCom.Interfaces.IABCD")    

    //]
    //interface IABCD : IDispatch {
    //    [id(0x60020000)]
    //    HRESULT B();
    //    [id(0x0000002a)]
    //    HRESULT D();
    //    [id(0x60020002)]
    //    HRESULT C();
    //    [id(0x60020003)]
    //    HRESULT A();
    //};

    [ComVisible(true)]
    [Guid("CD9A1BA0-621A-4D2A-A6CC-7B16EB79C3D3")]
    public interface IABCD
    {
        void B();
        [DispId(42)]
        void D();
        void C();
        void A();
    }
}
