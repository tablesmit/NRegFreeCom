using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace NRegFreeCom
{
    public interface IAssembly : IDisposable
    {
        string FullName { get; }

        string Location { get; }

        IntPtr LoadCompiledResource(uint name);

        /// <summary>
        /// Gets public function from native library.
        /// </summary>
        /// <typeparam name="T">
        /// Managed counterpart delegate of native function. 
        /// If <paramref name="defName"/> not provided then <typeparamref name="T"/> <see cref="MemberInfo.Name"/> is used for search.
        /// </typeparam>
        /// <paramref name="defName">Optional name of function. If not defined then name of<typeparamref name="T"/> is used</paramref>
        /// <returns></returns>
        /// <exception cref="EntryPointNotFoundException">No such export in library</exception>
        T GetDelegate<T>(string defName = null)
            where T : class, ISerializable, ICloneable; // is delegate

        /// <summary>
        /// Tries to get public function from native library. Returns false if no function was found.
        /// </summary>
        /// <typeparam name="T">
        /// Managed counterpart delegate of native function. 
        /// If <paramref name="defName"/> not provided then <typeparamref name="T"/> <see cref="MemberInfo.Name"/> is used for search.
        /// </typeparam>
        /// <paramref name="defName">Optional name of function. If not defined then name of<typeparamref name="T"/> is used</paramref>
        /// <returns></returns>
        bool TryGetDelegate<T>(out T nativeDelegate, string defName = null)
            where T : class, ISerializable, ICloneable; // is delegate

    }
}