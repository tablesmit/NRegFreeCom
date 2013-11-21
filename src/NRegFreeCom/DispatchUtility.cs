#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Security.Permissions;

#endregion

namespace TestDispatchUtility
{
    ///www.codeproject.com/Articles/523417/Reflection-with-IDispatch-based-COM-objects
    /// <summary>
    /// Provides helper methods for working with COM IDispatch objects that have a registered type library.
    /// </summary>
    public static class DispatchUtility
    {
        #region Private Constants

        private const int S_OK = 0; //From WinError.h
        private const int LOCALE_SYSTEM_DEFAULT = 2 << 10; //From WinNT.h == 2048 == 0x800

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets whether the specified object implements IDispatch.
        /// </summary>
        /// <param name="obj">An object to check.</param>
        /// <returns>True if the object implements IDispatch.  False otherwise.</returns>
        public static bool ImplementsIDispatch(object obj)
        {
            bool result = obj is IDispatchInfo;
            return result;
        }

        /// <summary>
        /// Gets a Type that can be used with reflection.
        /// </summary>
        /// <param name="obj">An object that implements IDispatch.</param>
        /// <param name="throwIfNotFound">Whether an exception should be thrown if a Type can't be obtained.</param>
        /// <returns>A .NET Type that can be used with reflection.</returns>
        /// <exception cref="InvalidCastException">If <paramref name="obj"/> doesn't implement IDispatch.</exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static Type GetType(object obj, bool throwIfNotFound)
        {
            RequireReference(obj, "obj");
            Type result = GetType((IDispatchInfo)obj, throwIfNotFound);
            return result;
        }

        /// <summary>
        /// Tries to get the DISPID for the requested member name.
        /// </summary>
        /// <param name="obj">An object that implements IDispatch.</param>
        /// <param name="name">The name of a member to lookup.</param>
        /// <param name="dispId">If the method returns true, this holds the DISPID on output.
        /// If the method returns false, this value should be ignored.</param>
        /// <returns>True if the member was found and resolved to a DISPID.  False otherwise.</returns>
        /// <exception cref="InvalidCastException">If <paramref name="obj"/> doesn't implement IDispatch.</exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool TryGetDispId(object obj, string name, out int dispId)
        {
            RequireReference(obj, "obj");
            bool result = TryGetDispId((IDispatchInfo)obj, name, out dispId);
            return result;
        }

        /// <summary>
        /// Invokes a member by DISPID.
        /// </summary>
        /// <param name="obj">An object that implements IDispatch.</param>
        /// <param name="dispId">The DISPID of a member.  This can be obtained using
        /// <see cref="TryGetDispId(object, string, out int)"/>.</param>
        /// <param name="args">The arguments to pass to the member.</param>
        /// <returns>The member's return value.</returns>
        /// <remarks>
        /// This can invoke a method or a property get accessor.
        /// </remarks>
        public static object Invoke(object obj, int dispId, object[] args)
        {
            string memberName = "[DispId=" + dispId + "]";
            object result = Invoke(obj, memberName, args);
            return result;
        }

        /// <summary>
        /// Invokes a member by name.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <param name="memberName">The name of the member to invoke.</param>
        /// <param name="args">The arguments to pass to the member.</param>
        /// <returns>The member's return value.</returns>
        /// <remarks>
        /// This can invoke a method or a property get accessor.
        /// </remarks>
        public static object Invoke(object obj, string memberName, object[] args)
        {
            RequireReference(obj, "obj");
            Type type = obj.GetType();
            object result = type.InvokeMember(memberName, BindingFlags.InvokeMethod | BindingFlags.GetProperty,
                null, obj, args, null);
            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Requires that the value is non-null.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="name">The name of the value.</param>
        private static void RequireReference<T>(T value, string name) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Gets a Type that can be used with reflection.
        /// </summary>
        /// <param name="dispatch">An object that implements IDispatch.</param>
        /// <param name="throwIfNotFound">Whether an exception should be thrown if a Type can't be obtained.</param>
        /// <returns>A .NET Type that can be used with reflection.</returns>
        private static Type GetType(IDispatchInfo dispatch, bool throwIfNotFound)
        {
            RequireReference(dispatch, "dispatch");

            Type result = null;
            int typeInfoCount;
            int hr = dispatch.GetTypeInfoCount(out typeInfoCount);
            if (hr == S_OK && typeInfoCount > 0)
            {
                // Type info isn't usually culture-aware for IDispatch, so we might as well pass
                // the default locale instead of looking up the current thread's LCID each time
                // (via CultureInfo.CurrentCulture.LCID).
                dispatch.GetTypeInfo(0, LOCALE_SYSTEM_DEFAULT, out result);
            }

            if (result == null && throwIfNotFound)
            {
                // If the GetTypeInfoCount called failed, throw an exception for that.
                Marshal.ThrowExceptionForHR(hr);

                // Otherwise, throw the same exception that Type.GetType would throw.
                throw new TypeLoadException();
            }

            return result;
        }

        /// <summary>
        /// Tries to get the DISPID for the requested member name.
        /// </summary>
        /// <param name="dispatch">An object that implements IDispatch.</param>
        /// <param name="name">The name of a member to lookup.</param>
        /// <param name="dispId">If the method returns true, this holds the DISPID on output.
        /// If the method returns false, this value should be ignored.</param>
        /// <returns>True if the member was found and resolved to a DISPID.  False otherwise.</returns>
        private static bool TryGetDispId(IDispatchInfo dispatch, string name, out int dispId)
        {
            RequireReference(dispatch, "dispatch");
            RequireReference(name, "name");

            bool result = false;

            // Members names aren't usually culture-aware for IDispatch, so we might as well
            // pass the default locale instead of looking up the current thread's LCID each time
            // (via CultureInfo.CurrentCulture.LCID).
            Guid iidNull = Guid.Empty;
            int hr = dispatch.GetDispId(ref iidNull, ref name, 1, LOCALE_SYSTEM_DEFAULT, out dispId);

            const int DISP_E_UNKNOWNNAME = unchecked((int)0x80020006); //From WinError.h
            const int DISPID_UNKNOWN = -1; //From OAIdl.idl
            if (hr == S_OK)
            {
                result = true;
            }
            else if (hr == DISP_E_UNKNOWNNAME && dispId == DISPID_UNKNOWN)
            {
                // This is the only supported "error" case because it means IDispatch
                // is saying it doesn't know the member we asked about.
                result = false;
            }
            else
            {
                // The other documented result codes are all errors.
                Marshal.ThrowExceptionForHR(hr);
            }

            return result;
        }

        #endregion

     
    }
}
