using System;
using System.Runtime.InteropServices;
using NRegFreeCom.Interop.ComTypes;

namespace NRegFreeCom.Interop
{
    public static class DEF_Objbase
    {
        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int DllGetClassObject(ref Guid clsid, ref Guid iid, [Out, MarshalAs(UnmanagedType.Interface)] out IClassFactory_AutoMarshal classFactory);
    }
}