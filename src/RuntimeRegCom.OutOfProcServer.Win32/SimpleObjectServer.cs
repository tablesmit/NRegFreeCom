

using System;
using NRegFreeCom;
using NRegFreeCom.Interop;
using RegFreeCom.Interfaces;

namespace RuntimeRegCom.OutOfProcServer.Win32
{



    ///<seealso cref="All In One Code Framework CSExeComServer"/>
    sealed internal class SimpleObjectServer
    {
        private readonly IDispatcher _dispatcher;

        public SimpleObjectServer(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        private object syncRoot = new Object(); // For thread-sync in lock
        private bool _bRunning = false; // Whether the server is running
        private uint _cookieSimpleObj;
        private bool IPC_GC = true;
 


        /// <summary>
        /// PreMessageLoop is responsible for registering the COM class 
        /// factories for the COM classes to be exposed from the server, and 
        /// initializing the key member variables of the COM server.
        /// </summary>
        private void Register()
        {
            //
            // Register the COM class factories.
            // 
            
            var clsidSimpleObj = new Guid(SimpleObjectId.ClassId);

            // Register the SimpleObject class object
            var factory = new SimpleObjectClassFactory();
            factory.NoReferenceEvent += _reference_NoReferenceEvent;
            int hResult = NativeMethods.CoRegisterClassObject(
                ref clsidSimpleObj,                 // CLSID to be registered
                factory,
                CLSCTX.LOCAL_SERVER,                // Context to run
                REGCLS.MULTIPLEUSE | REGCLS.SUSPENDED,
                out _cookieSimpleObj);
            if (hResult != 0)
            {
                throw new ApplicationException(
                    "CoRegisterClassObject failed w/err 0x" + hResult.ToString("X"));
            }

            // Register other class objects 
            // ...

            // Inform the SCM about all the registered classes, and begins 
            // letting activation requests into the server process.
            hResult = NativeMethods.CoResumeClassObjects();
            if (hResult != 0)
            {
                // Revoke the registration of SimpleObject on failure
                if (_cookieSimpleObj != 0)
                {
                    NativeMethods.CoRevokeClassObject(_cookieSimpleObj);
                }

                // Revoke the registration of other classes
                // ...

                throw new ApplicationException(
                    "CoResumeClassObjects failed w/err 0x" + hResult.ToString("X"));
            }
        }

        void _reference_NoReferenceEvent(object sender, EventArgs e)
        {
            Console.WriteLine("No reference to server");
            if (IPC_GC)
                _dispatcher.Invoke(new Action(UnRegister));
                _dispatcher.InvokeShutdown();
        }

    

        /// <summary>
        /// PostMessageLoop is called to revoke the registration of the COM 
        /// classes exposed from the server, and perform the cleanups.
        /// </summary>
        private void UnRegister()
        {
            // 
            // Revoke the registration of the COM classes.
            // 

            // Revoke the registration of SimpleObject
            if (_cookieSimpleObj != 0)
            {
                NativeMethods.CoRevokeClassObject(_cookieSimpleObj);
            }
        }

        /// <summary>
        /// Run the COM server. If the server is running, the function 
        /// returns directly.
        /// </summary>
        /// <remarks>The method is thread-safe.</remarks>
        public void Run()
        {
            lock (syncRoot) // Ensure thread-safe
            {
                // If the server is running, return directly.
                if (_bRunning)
                    return;

                // Indicate that the server is running now.
                _bRunning = true;
            }

            try
            {
                Register();
            }
            finally
            {
                _bRunning = false;
            }
        }
    }
}