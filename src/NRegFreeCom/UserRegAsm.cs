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
 
        public void RegisterInProcServer(Type t,RegistryView registryView = RegistryView.Default)
        {
            var reg = ClrComRegistryInfo.Create(t);
            #if NET35
            throw new NotImplementedException("Need to backport 4.0 methods");
#else
            var root = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, registryView);
            var classes = root.CreateSubKey(CLASSES);
            registerInProcServer(classes, reg);
#endif
        }

        public void UnregisterInProcServer(Type t,RegistryView registryView = RegistryView.Default)
        {
            var reg = ClrComRegistryInfo.Create(t);
            #if NET35
            throw new NotImplementedException("Need to backport 4.0 methods");
#else
            var root = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, registryView);
            var classes = root.CreateSubKey(CLASSES);
            unregisterInProcServer(classes, reg);
#endif
        }


    }
}