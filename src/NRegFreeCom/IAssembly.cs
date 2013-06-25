using System;

namespace NRegFreeCom
{
    public interface IAssembly: IDisposable
    {
        string FullName { get; }

        string Location { get; }

        IntPtr LoadCompiledResource(uint name);

    }
}