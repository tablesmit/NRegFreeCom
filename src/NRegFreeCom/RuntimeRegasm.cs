using System;
using System.Reflection;
using Microsoft.Win32;

namespace NRegFreeCom
{
    public class RuntimeRegasm
    {
        /// <summary>
        /// Register the component as a local server.
        /// </summary>
        /// <param name="t"></param>
        public static void RegasmRegisterLocalServer(Type t)
        {
            GuardNullType(t, "t");  // Check the argument

            // Open the CLSID key of the component.
            using (RegistryKey keyCLSID = Registry.ClassesRoot.OpenSubKey(
                @"CLSID\" + t.GUID.ToString("B"), /*writable*/true))
            {
                // Remove the auto-generated InprocServer32 key after registration
                // (REGASM puts it there but we are going out-of-proc).
                keyCLSID.DeleteSubKeyTree("InprocServer32");

                // Create "LocalServer32" under the CLSID key
                using (RegistryKey subkey = keyCLSID.CreateSubKey("LocalServer32"))
                {
                    subkey.SetValue("", Assembly.GetExecutingAssembly().Location,
                                    RegistryValueKind.String);
                }
            }
        }

        /// <summary>
        /// Unregister the component.
        /// </summary>
        /// <param name="t"></param>
        public static void RegasmUnregisterLocalServer(Type t)
        {
            GuardNullType(t, "t");  // Check the argument

            // Delete the CLSID key of the component
            Registry.ClassesRoot.DeleteSubKeyTree(@"CLSID\" + t.GUID.ToString("B"));
        }

        private static void GuardNullType(Type t, String param)
        {
            if (t == null)
            {
                throw new ArgumentException("The CLR type must be specified.", param);
            }
        }
    }
}