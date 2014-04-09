using System;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace NRegFreeCom
{
    /// <summary>
    /// Registers and unregisters COM objects.
    /// </summary>
    public class RegAsm
    {

        protected const string CLASSES = @"SOFTWARE\Classes\";
        protected const string CLSID = @"CLSID\";
        protected const string INTERFACE = @"Interface\";

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

        /// <summary>
        /// If this is true, then some registry calls are redirected.
        /// </summary>
        /// <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/aa384232.aspx"/>
        public static bool IsWoW64RedirectionOn { get { return Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess; } }

        protected static void registerInProcServer(RegistryKey classes, ClrComRegistryInfo reg)
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
                            inprocServer32.SetValue("", reg.NetEntryPoint);
                            inprocServer32.SetValue("ThreadingModel", reg.ThreadingModel);
                            inprocServer32.SetValue("Class", reg.Class);
                            inprocServer32.SetValue("RuntimeVersion", reg.NetVersion);
                            inprocServer32.SetValue("Assembly", reg.Assembly.FullName);
                            using (RegistryKey version = inprocServer32.CreateSubKey(reg.Assembly.GetName().Version.ToString()))
                            {
                                version.SetValue("Class", reg.Class);
                                version.SetValue("Assembly", reg.Assembly.FullName);
                                version.SetValue("RuntimeVersion", reg.NetVersion);
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

        protected static void unregisterInProcServer(RegistryKey classes, ClrComRegistryInfo reg)
        {
            using (classes)
            {
                using (
                    RegistryKey clsidKey = classes.OpenSubKey(CLSID, RegistryKeyPermissionCheck.ReadWriteSubTree,
                        RegistryRights.Delete))
                {
                    if (clsidKey != null) //NOTE: to be safe if can happen clean machine without any user specific installation
                        clsidKey.DeleteSubKeyTree(reg.Guid, false);
                }
                classes.DeleteSubKeyTree(reg.ProgId, false);
            }
        }
    }
}