using System;

using System.ComponentModel;

using System.Runtime.InteropServices;
using NRegFreeCom.Interop;


namespace NRegFreeCom
{
    public static class ComMarshall 
    {
        /// <summary>
        /// Gets the real COM object out of remoting proxy. If COM object was passed across AppDomains it was proxied. 
        /// </summary>
        /// <remarks>
        /// Better to unproxy and call real COM (its RCW).
        /// Fixes problem of dead COM RCW after some time withoyt any calls to COM.
        /// </remarks>
        /// <typeparam name="T">The type of COM object.</typeparam>
        /// <param name="instance">The instance of remoting proxy.</param>
        /// <returns> The instance of a type that represents a COM object.</returns>
        public static T GetRealObjectByProxy<T>(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            Guid typeIdd = typeof(T).GUID;
            IntPtr unknownPointer = Marshal.GetIUnknownForObject(instance);
            IntPtr realPointer = IntPtr.Zero;
            int result = Marshal.QueryInterface(unknownPointer, ref typeIdd, out realPointer);
            if (result != SYSTEM_ERROR_CODES.ERROR_SUCCESS)
                throw new InvalidCastException(string.Format("Failed to cast COM proxy to real object of type {0}", typeof(T)), new Win32Exception(result));
            return (T)Marshal.GetObjectForIUnknown(realPointer);
        }
    }
}