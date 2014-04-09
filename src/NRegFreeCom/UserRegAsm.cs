using System;
using Microsoft.Win32;

namespace NRegFreeCom
{

    public class UserRegAsm :RegAsm, IRegAsm
    {


        public void RegisterLocalServer(Type t)
        {

        }


        public void UnregisterLocalServer(Type t)
        {

        }

        //TODO: Support 32 bit process to register 64 bit entries and vice versa (Wow6432Node)
        //TODO: pass some abstract config obtained from type 
        public void RegisterInProcServer(Type t,RegistryView registryView = RegistryView.Default)
        {
            var reg = ClrComRegistryInfo.Create(t);
            var root = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, registryView);
            var classes = root.CreateSubKey(CLASSES);
            registerInProcServer(classes, reg);
        }

    

        //TODO: Support 32 bit process to delete 64 bit entries and vice versa (Wow6432Node)
        public void UnregisterInProcServer(Type t,RegistryView registryView = RegistryView.Default)
        {
            var reg = ClrComRegistryInfo.Create(t);
            var root = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, registryView);
            var classes = root.CreateSubKey(CLASSES);
            unregisterInProcServer(classes, reg);
        }


    }
}