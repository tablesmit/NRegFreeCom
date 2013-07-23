using System;
using System.Reflection;
using Microsoft.Win32;

namespace NRegFreeCom
{
    /// <summary>
    /// Registers and unregisters COM objects for current user.
    /// </summary>
    public class Regasm
    {
        /// <summary>
        /// Register the component as a local server.
        /// </summary>
        /// <param name="t"></param>
        public static void RegisterLocalServer(Type t)
        {
            if (t == null)
            {
                throw new ArgumentException("The CLR type must be specified.", "t");
            }
            //TODO: ensure that most resticed is used, may be HKEY_CURRENT_USER\Software\Classes\Interface
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
                    subkey.SetValue("", System.Reflection.Assembly.GetExecutingAssembly().Location,
                                    RegistryValueKind.String);
                }
            }
        }

        /// <summary>
        /// Unregister the component.
        /// </summary>
        /// <param name="t"></param>
        public static void UnregisterLocalServer(Type t)
        {
            if (t == null)
            {
                throw new ArgumentException("The CLR type must be specified.", "t");
            }

            // Delete the CLSID key of the component
            Registry.ClassesRoot.DeleteSubKeyTree(@"CLSID\" + t.GUID.ToString("B"));
        }
    }
}