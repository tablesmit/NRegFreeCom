using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RegFreeCom.Interfaces
{
    public class RegFreeComIds
{
    public const string CLSID = "9C21B7EB-7E27-4405-BCE0-62B338DF83BB";
}

    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("EEE50CDF-D8EC-4F38-B986-C231EC45171E")]
    public interface IRegFreeCom
    {
        [ComVisible(true)]
        string GetString(int number);
    }
}
