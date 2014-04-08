using System;

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

        void RegisterInProcSever(Type t, string customProgId = null);

        void UnregisterInProcSever(Type t, string customProgId = null);
    }
}