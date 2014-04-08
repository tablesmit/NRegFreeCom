using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace NRegFreeCom
{

    public class UserRegAsm : IRegAsm
    {
        private const string CLASSES = @"SOFTWARE\Classes\";
        private const string CLSID = @"CLSID\";
        private const string INTERFACE = @"Interface\";

        public void RegisterLocalServer(Type t)
        {

        }


        public void UnregisterLocalServer(Type t)
        {

        }

        //TODO: Support 32 bit process to register 64 bit entries and vice versa (Wow6432Node)
        //TODO: pass some abstract config obtained from type 
        public void RegisterInProcSever(Type t, string customProgId = null)
        {
            var reg = ClrComRegistryInfo.Create(t, customProgId);
            using (RegistryKey classes = Registry.CurrentUser.CreateSubKey(CLASSES))
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
                            using (categories.CreateSubKey("{62C8FE65-4EBB-45E7-B440-6E39B2CDBF29}")) { }
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

        //TODO: Support 32 bit process to delete 64 bit entries and vice versa (Wow6432Node)
        public void UnregisterInProcSever(Type t, string customProgId = null)
        {
            var reg = ClrComRegistryInfo.Create(t, customProgId);
            using (RegistryKey classes = Registry.CurrentUser.CreateSubKey(CLASSES))
            {
                using (RegistryKey clsidKey = classes.OpenSubKey(CLSID,RegistryKeyPermissionCheck.ReadWriteSubTree,RegistryRights.Delete))
                {
                    if (clsidKey != null)//NOTE: to be safe if can happen clean machine without any user specific installation
                        clsidKey.DeleteSubKeyTree(reg.Guid,false);
                }
                classes.DeleteSubKeyTree(reg.ProgId,false);
            }
        }
    }
}