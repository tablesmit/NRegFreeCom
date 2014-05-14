using System;
using System.Runtime.InteropServices.ComTypes;
using System.Security.AccessControl;
using System.Xml.Linq;
using Microsoft.Win32;

namespace NRegFreeCom
{
    internal static class ComRegistryExtensions
    {
        public static RegistryKey OpenSubKeyDeletion(this RegistryKey parent, string name)
        {
            return parent.OpenSubKey(name, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.Delete);
        }

        public static string ToRegistry(this Guid current)
        {
            return current.ToString("B");
        }
    }

    /// <summary>
    /// Registers and unregisters COM objects.
    /// </summary>
    public class RegAsm
    {

        protected const string CLASSES = @"SOFTWARE\Classes\";
        protected const string CLSID = @"CLSID\";
        protected const string INTERFACE = @"Interface\";
        protected const string ProxyStubClsid32Key = "ProxyStubClsid32";
        protected const string TYPE_LIB = @"TypeLib\";

        private static IRegAsm _user = new UserRegAsm();
        private static IRegAsm _machine = new MachineRegAsm();

        public static IRegAsm User
        {
            get { return _user; }

        }

        public static IRegAsm Machine
        {
            get { return _machine; }

        }

        protected void registerTypeLib(RegistryKey classes, ITypeLibAttributes reg)
        {
            using (classes)
            {
                using (RegistryKey tlbKey = classes.CreateSubKey(TYPE_LIB))
                {
                    using (RegistryKey guidKey = tlbKey.CreateSubKey(reg.Guid.ToRegistry()))
                    {
                        using (RegistryKey verKey = guidKey.CreateSubKey(reg.Version.ToString()))
                        {
                            //TODO: verKey.SetValue("", contnet of AssemblyDescriptionAttr);
                            //TODO: are any flags needed?
                            //using (RegistryKey flags = verKey.CreateSubKey("FLAGS")){
                            //flags.SetValue("","0");
                            //}
                        }
                    }
                }
            }
        }

        protected void unregisterTypeLib(RegistryKey classes, ITypeLibAttributes reg)
        {
            using (classes)
            {
                using (RegistryKey tlbKey = classes.OpenSubKeyDeletion(TYPE_LIB))
                {
                    if (tlbKey != null) //NOTE: to be safe if can happen clean machine without any user specific installation
                        tlbKey.DeleteSubKeyTree(reg.Guid.ToRegistry(), false);
                }
            }
        }

        protected void registerInterface(RegistryKey classes, ComInterfaceInfo reg)
        {
            using (classes)
            {
                using (RegistryKey infKey = classes.CreateSubKey(INTERFACE))
                {
                    using (RegistryKey guidKey = infKey.CreateSubKey(reg.Guid))
                    {

                        //some PSDispatch oleaut32 value needed
                        using (RegistryKey ps32 = guidKey.CreateSubKey(ProxyStubClsid32Key))
                        {
                            ps32.SetValue("", "{00020420-0000-0000-C000-000000000046}");
                        }

                        using (RegistryKey typeLibKey = guidKey.CreateSubKey(TYPE_LIB))
                        {
                            typeLibKey.SetValue("", reg.TypeLib.Guid.ToString("B"));//with curly braces
                            typeLibKey.SetValue("Version", reg.TypeLib.Version);
                        }
                    }
                }
            }
        }



        protected void unregisterInterface(RegistryKey classes, ComInterfaceInfo reg)
        {
            using (classes)
            {
                using (RegistryKey interfaceKey = classes.OpenSubKeyDeletion(INTERFACE))
                {
                    if (interfaceKey != null) //NOTE: to be safe if can happen clean machine without any user specific installation
                        interfaceKey.DeleteSubKeyTree(reg.Guid, false);
                }
            }
        }

        protected static void registerInProcServer(RegistryKey classes, ComClassInfo reg)
        {
            using (classes)
            {
                using (RegistryKey clsidKey = classes.CreateSubKey(CLSID))
                {
                    using (RegistryKey guidKey = clsidKey.CreateSubKey(reg.Guid))
                    {
                        guidKey.SetValue("", reg.Class);
                        using (RegistryKey inprocServer32 = guidKey.CreateSubKey("InprocServer32"))
                        {
                            inprocServer32.SetValue("", reg.RuntimeEntryPoint);
                            inprocServer32.SetValue("ThreadingModel", reg.ThreadingModel);
                            inprocServer32.SetValue("Class", reg.Class);
                            inprocServer32.SetValue("RuntimeVersion", reg.RuntimeVersion);
                            inprocServer32.SetValue("Assembly", reg.Assembly.FullName);
                            using (RegistryKey version = inprocServer32.CreateSubKey(reg.Assembly.GetName().Version.ToString()))
                            {
                                version.SetValue("Class", reg.Class);
                                version.SetValue("Assembly", reg.Assembly.FullName);
                                version.SetValue("RuntimeVersion", reg.RuntimeVersion);
                            }
                        }
                        using (RegistryKey progIdKey = guidKey.CreateSubKey("ProgId"))
                        {
                            progIdKey.SetValue("", reg.ProgId);
                        }
                        using (RegistryKey categories = guidKey.CreateSubKey("Implemented Categories"))
                        {
                            using (categories.CreateSubKey("{62C8FE65-4EBB-45E7-B440-6E39B2CDBF29}"))
                            {
                            }
                        }
                    }
                }
                using (RegistryKey prodIdKey = classes.CreateSubKey(reg.ProgId))
                {
                    prodIdKey.SetValue("", reg.Class);
                    using (RegistryKey prodIdToClassId = prodIdKey.CreateSubKey(CLSID))
                    {
                        prodIdToClassId.SetValue("", reg.Guid);
                    }
                }
            }
        }

        protected static void unregisterInProcServer(RegistryKey classes, ComClassInfo reg)
        {
            using (classes)
            {
                using (RegistryKey clsidKey = classes.OpenSubKeyDeletion(CLSID))
                {
                    if (clsidKey != null) //NOTE: to be safe if can happen clean machine without any user specific installation
                        clsidKey.DeleteSubKeyTree(reg.Guid, false);
                }
                classes.DeleteSubKeyTree(reg.ProgId, false);
            }
        }
    }
}