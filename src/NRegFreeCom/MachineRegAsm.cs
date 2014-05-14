using System;
using Microsoft.Win32;


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
         
            using (RegistryKey keyCLSID = Registry.ClassesRoot.OpenSubKey(RegAsm.CLSID + t.GUID.ToString("B"), /*writable*/true))
            {
                // Remove the auto-generated InprocServer32 key after registration
                // (REGASM puts it there but we are going out-of-proc).
                keyCLSID.DeleteSubKeyTree("InprocServer32",false);

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
            Registry.ClassesRoot.DeleteSubKeyTree(RegAsm.CLSID + t.GUID.ToString("B"));
        }

        public void RegisterInProcServer(Type t, RegistryView registryView = RegistryView.Default)
        {
            var reg = ComClrInfoFactory.CreateClass(t);
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
            var reg = ComClrInfoFactory.CreateClass(t);
         #if NET35
            throw new NotImplementedException("Need to backport 4.0 methods");
#else   
            var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            var classes = root.CreateSubKey(CLASSES);
            unregisterInProcServer(classes, reg);
#endif
        }


     
    	
		public void RegisterRecord(Type type, RegistryView registryView)
		{
			throw new NotImplementedException();
		}
    	
		public void RegisterInterface(Type type, RegistryView registryView)
		{
			throw new NotImplementedException();
		}

        public void RegisterTypeLib(System.Reflection.Assembly typeLib, RegistryView registryView= RegistryView.Default)
		{
			throw new NotImplementedException();
		}

        public void UnregisterInterface(Type type, RegistryView registryView = RegistryView.Default)
        {
            throw new NotImplementedException();
        }

        public void UnregisterTypeLib(System.Reflection.Assembly typeLib, RegistryView registryView = RegistryView.Default)
        {
            throw new NotImplementedException();
        }
    }
}