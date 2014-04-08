using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using RegFreeCom.Interfaces;

namespace NRegFreeCom.Tests
{
    public static class RuntimeRegServerConstants
    {
        public const string CLSID = "BEEBB958-3CFD-484A-8646-19163C74C5EA";
        public const string IID = "51A6FCFE-3106-4222-89BE-CA83AB29E7E7";
    }


    [ComVisible(true)]
    [Guid(RuntimeRegServerConstants.CLSID)]
    [ComDefaultInterface(typeof(IRegFreeCom))]
    public class RuntimeRegServer:IRuntimeRegServer
    {
    }

    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid(RuntimeRegServerConstants.IID)]
    public interface IRuntimeRegServer
    {
    }
}
