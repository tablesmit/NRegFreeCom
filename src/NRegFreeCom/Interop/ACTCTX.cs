using System;
using System.Runtime.InteropServices;

namespace NRegFreeCom.Interop
{
    /// <summary>
    /// Activation context structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    internal struct ACTCTX
    {
        public Int32 cbSize;
        public UInt32 dwFlags;
        public string lpSource;
        public UInt16 wProcessorArchitecture;
        public UInt16 wLangId;
        public string lpAssemblyDirectory;
        public string lpResourceName;
        public string lpApplicationName;
        public IntPtr hModule;
    }
}