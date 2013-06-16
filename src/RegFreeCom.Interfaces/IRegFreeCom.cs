using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RegFreeCom.Interfaces
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("EEE50CDF-D8EC-4F38-B986-C231EC45171E")]
    public interface IRegFreeCom
    {
        [ComVisible(true)]
        string Info { get; }

        [ComVisible(true)]
        void Request(string hello);

        [ComVisible(true)]
        void Ping();

        [ComVisible(true)]
        int Answer();
    }
}
