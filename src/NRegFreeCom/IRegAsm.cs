using System;
using Microsoft.Win32;

namespace NRegFreeCom
{
    public interface IRegAsm
    {
        /// <summary>
        /// Register the component as a local server.
        /// </summary>
        /// <param name="t"></param>
        void RegisterLocalServer(Type t);

        /// <summary>
        /// Unregister the component.
        /// </summary>
        /// <param name="t"></param>
        void UnregisterLocalServer(Type t);


        void RegisterInProcServer(Type t, RegistryView registryView = RegistryView.Default);

        void UnregisterInProcServer(Type t,RegistryView registryView = RegistryView.Default);
    }
}