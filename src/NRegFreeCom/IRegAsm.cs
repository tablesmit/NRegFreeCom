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
        /// Unregisters the component.
        /// </summary>
        /// <param name="t"></param>
        void UnregisterLocalServer(Type t);


        void RegisterInProcServer(Type t, RegistryView registryView = RegistryView.Default);

        void UnregisterInProcServer(Type t,RegistryView registryView = RegistryView.Default);

        /// <summary>
        /// Registers any <seealso cref="ValueType"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="registryView"></param>
        void RegisterRecord(Type type, RegistryView registryView =  RegistryView.Default);

        /// <summary>
        /// Registers COM visible CLR interface.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="registryView"></param>
        void RegisterInterface(Type type, RegistryView registryView = RegistryView.Default);
        
        /// <summary>
        /// Registers CLR assembly as COM TypeLib.
        /// </summary>
        /// <param name="registryView"></param>
        void RegisterTypeLib(System.Reflection.Assembly typeLib, RegistryView registryView = RegistryView.Default);
    }
}