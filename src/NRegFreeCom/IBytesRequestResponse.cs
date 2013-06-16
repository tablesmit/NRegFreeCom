using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NRegFreeCom
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("6B214614-E107-4028-AEAA-D8CF80915CD0")]
    public interface IBytesRequestResponse
    {
        /// <summary>
        /// Allows to request COM object with bytes and be responded with bytes.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        byte[] Execute(byte[] request);
    }
}
