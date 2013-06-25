using System;
using System.Runtime.Serialization;

namespace NRegFreeCom
{
    public interface IAssembly: IDisposable
    {
        string FullName { get; }

        string Location { get; }

        IntPtr LoadCompiledResource(uint name);

        /// <summary>
        /// Gets public method in native library.
        /// </summary>
        /// <typeparam name="T">delegate</typeparam>
        /// <returns></returns>
        /// <exception cref="EntryPointNotFoundException">No such export in library</exception>
        T GetDelegate<T>(string defName = null)
            where T : class, ISerializable, ICloneable; // is delegate

    }
}