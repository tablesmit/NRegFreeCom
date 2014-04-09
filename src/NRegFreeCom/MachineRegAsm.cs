using System;
using System.Security;
using Microsoft.Win32;

#if NET35
namespace Microsoft.Win32
{
    /// <summary>
    /// Specifies which registry view to target on a 64-bit operating system.
    /// </summary>
    public enum RegistryView
    {
        Default = 0,
        Registry64 = 256,
        Registry32 = 512,
    }
}
#endif
namespace NRegFreeCom
{
    public class MachineRegAsm : RegAsm,IRegAsm
    {

        public void RegisterLocalServer(Type t)
        {
            if (t == null)
            {
                throw new ArgumentException("The CLR type must be specified.", "t");
            }
            //TODO: ensure that most resticed is used, may be HKEY_CURRENT_USER\Software\Classes\Interface
            // Open the CLSID key of the component.
         
            using (RegistryKey keyCLSID = Registry.ClassesRoot.OpenSubKey(@"CLSID\" + t.GUID.ToString("B"), /*writable*/true))
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


        public  void UnregisterLocalServer(Type t)
        {
            if (t == null)
            {
                throw new ArgumentException("The CLR type must be specified.", "t");
            }

            // Delete the CLSID key of the component
            Registry.ClassesRoot.DeleteSubKeyTree(@"CLSID\" + t.GUID.ToString("B"));
        }

        public void RegisterInProcServer(Type t, RegistryView registryView = RegistryView.Default)
        {
            var reg = ClrComRegistryInfo.Create(t);
#if NET35
            throw new NotImplementedException("Need to backport 4.0 methods");
#else
            var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            var classes = root.CreateSubKey(CLASSES);
            registerInProcServer(classes, reg);
#endif
        }

        public void UnregisterInProcServer(Type t, RegistryView registryView = RegistryView.Default)
        {
            var reg = ClrComRegistryInfo.Create(t);
         #if NET35
            throw new NotImplementedException("Need to ");
#else   
            var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            var classes = root.CreateSubKey(CLASSES);
            unregisterInProcServer(classes, reg);
#endif
        }


     
    }
}