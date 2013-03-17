using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;

namespace NRegFreeCom
{
    //[SuppressUnmanagedCodeSecurity]
    public static class NativeMethods
    {
        ///<seealso href="http://search.microsoft.com/en-US/results.aspx?q=SetDllDirectory"/>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(string lpPathName); 

        [DllImport("user32.dll", CharSet = CharSet.Auto,SetLastError = true)]
        public static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr FindResource(IntPtr hModule, uint lpName, uint lpType);

        [DllImport("kernel32.dll", EntryPoint = "LoadAnyCpuLibrary",SetLastError = true)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress",SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary",SetLastError = true)]
        public static extern bool FreeLibrary(int hModule);

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern object CoGetClassObject(
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
           CLSCTX dwClsContext,
           IntPtr pServerInfo,
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);


        [DllImport("kernel32.dll", EntryPoint = "EnumResourceNamesW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumResourceNames(
          IntPtr hModule,
          uint lpszType,
          ENUMRESNAMEPROC lpEnumFunc,
          IntPtr lParam);

    

        /// <summary>
        /// CoInitializeEx() can be used to set the apartment model of individual 
        /// threads.
        /// </summary>
        /// <param name="pvReserved">Must be NULL</param>
        /// <param name="dwCoInit">
        /// The concurrency model and initialization options for the thread
        /// </param>
        /// <returns></returns>
        [DllImport("ole32.dll")]
        public static extern int CoInitializeEx(IntPtr pvReserved, uint dwCoInit);

        /// <summary>
        /// CoUninitialize() is used to uninitialize a COM thread.
        /// </summary>
        [DllImport("ole32.dll")]
        public static extern void CoUninitialize();

        /// <summary>
        /// Registers an EXE class object with OLE so other applications can 
        /// connect to it. EXE object applications should call 
        /// CoRegisterClassObject on startup. It can also be used to register 
        /// internal objects for use by the same EXE or other code (such as DLLs)
        /// that the EXE uses.
        /// </summary>
        /// <param name="rclsid">CLSID to be registered</param>
        /// <param name="pUnk">
        /// Pointer to the IUnknown interface on the class object whose 
        /// availability is being published.
        /// </param>
        /// <param name="dwClsContext">
        /// Context in which the executable code is to be run.
        /// </param>
        /// <param name="flags">
        /// How connections are made to the class object.
        /// </param>
        /// <param name="lpdwRegister">
        /// Pointer to a value that identifies the class object registered; 
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// PInvoking CoRegisterClassObject to register COM objects is not 
        /// supported.
        /// </remarks>
        [DllImport("ole32.dll")]
        public static extern int CoRegisterClassObject(
            ref Guid rclsid,
            [MarshalAs(UnmanagedType.Interface)] IClassFactory pUnk,
            CLSCTX dwClsContext,
            REGCLS flags,
            out uint lpdwRegister);

        /// <summary>
        /// Informs OLE that a class object, previously registered with the 
        /// CoRegisterClassObject function, is no longer available for use.
        /// </summary>
        /// <param name="dwRegister">
        /// Token previously returned from the CoRegisterClassObject function
        /// </param>
        /// <returns></returns>
        [DllImport("ole32.dll")]
        public static extern UInt32 CoRevokeClassObject(uint dwRegister);

        /// <summary>
        /// Called by a server that can register multiple class objects to inform 
        /// the SCM about all registered classes, and permits activation requests 
        /// for those class objects.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Servers that can register multiple class objects call 
        /// CoResumeClassObjects once, after having first called 
        /// CoRegisterClassObject, specifying REGCLS_LOCAL_SERVER | 
        /// REGCLS_SUSPENDED for each CLSID the server supports. This function 
        /// causes OLE to inform the SCM about all the registered classes, and 
        /// begins letting activation requests into the server process.
        /// 
        /// This reduces the overall registration time, and thus the server 
        /// application startup time, by making a single call to the SCM, no 
        /// matter how many CLSIDs are registered for the server. Another 
        /// advantage is that if the server has multiple apartments with 
        /// different CLSIDs registered in different apartments, or is a free-
        /// threaded server, no activation requests will come in until the server 
        /// calls CoResumeClassObjects. This gives the server a chance to 
        /// register all of its CLSIDs and get properly set up before having to 
        /// deal with activation requests, and possibly shutdown requests. 
        /// </remarks>
        [DllImport("ole32.dll")]
        public static extern int CoResumeClassObjects();



        /// <summary>
        /// Interface Id of IUnknown
        /// </summary>
        public const string IID_IUnknown =
            "00000000-0000-0000-C000-000000000046";

        /// <summary>
        /// Interface Id of IDispatch
        /// </summary>
        public const string IID_IDispatch =
            "00020400-0000-0000-C000-000000000046";

        /// <summary>
        /// Class does not support aggregation (or class object is remote)
        /// </summary>
        public const int CLASS_E_NOAGGREGATION = unchecked((int)0x80040110);

        /// <summary>
        /// No such interface supported
        /// </summary>
        public const int E_NOINTERFACE = unchecked((int)0x80004002);

        /// <summary>
        /// Get current thread ID.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        /// <summary>
        /// Get current process ID.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentProcessId();

        /// <summary>
        /// The GetMessage function retrieves a message from the calling thread's 
        /// message queue. The function dispatches incoming sent messages until a 
        /// posted message is available for retrieval. 
        /// </summary>
        /// <param name="lpMsg">
        /// Pointer to an MSG structure that receives message information from 
        /// the thread's message queue.
        /// </param>
        /// <param name="hWnd">
        /// Handle to the window whose messages are to be retrieved.
        /// </param>
        /// <param name="wMsgFilterMin">
        /// Specifies the integer value of the lowest message value to be 
        /// retrieved. 
        /// </param>
        /// <param name="wMsgFilterMax">
        /// Specifies the integer value of the highest message value to be 
        /// retrieved.
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetMessage(
            out MSG lpMsg,
            IntPtr hWnd,
            uint wMsgFilterMin,
            uint wMsgFilterMax);

        /// <summary>
        /// The TranslateMessage function translates virtual-key messages into 
        /// character messages. The character messages are posted to the calling 
        /// thread's message queue, to be read the next time the thread calls the 
        /// GetMessage or PeekMessage function.
        /// </summary>
        /// <param name="lpMsg"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);

        /// <summary>
        /// The DispatchMessage function dispatches a message to a window 
        /// procedure. It is typically used to dispatch a message retrieved by 
        /// the GetMessage function.
        /// </summary>
        /// <param name="lpMsg"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref MSG lpMsg);

        /// <summary>
        /// The PostThreadMessage function posts a message to the message queue 
        /// of the specified thread. It returns without waiting for the thread to 
        /// process the message.
        /// </summary>
        /// <param name="idThread">
        /// Identifier of the thread to which the message is to be posted.
        /// </param>
        /// <param name="Msg">Specifies the type of message to be posted.</param>
        /// <param name="wParam">
        /// Specifies additional message-specific information.
        /// </param>
        /// <param name="lParam">
        /// Specifies additional message-specific information.
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool PostThreadMessage(
            uint idThread,
            uint Msg,
            UIntPtr wParam,
            IntPtr lParam);


        /// <summary>
        /// Returns a pointer to the IRunningObjectTable
        /// interface on the local running object table (ROT).
        /// </summary>
        /// <param name="reserved">This parameter is reserved and must be 0.</param>
        /// <param name="prot">The address of an IRunningObjectTable* pointer variable
        /// that receives the interface pointer to the local ROT. When the function is
        /// successful, the caller is responsible for calling Release on the interface
        /// pointer. If an error occurs, *pprot is undefined.</param>
        /// <returns>This function can return the standard return values E_UNEXPECTED and S_OK.</returns>
        [DllImport("ole32.dll")]
        public static extern int GetRunningObjectTable(uint reserved, out System.Runtime.InteropServices.ComTypes.IRunningObjectTable pprot);

        [DllImport("ole32.dll")]
        public static extern int CreateFileMoniker([MarshalAs(UnmanagedType.LPWStr)] string lpszPathName, out System.Runtime.InteropServices.ComTypes.IMoniker ppmk);

        [DllImport("oleaut32.dll")]
        public static extern int RevokeActiveObject(int register, IntPtr reserved);

      

        [DllImport("ole32.dll")]
        public static extern int CreateClassMoniker([In] ref Guid rclsid,
           out IMoniker ppmk);

        // Activation Context API Functions
        [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "CreateActCtxW")]
        internal extern static IntPtr CreateActCtx(ref ACTCTX actctx);

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ActivateActCtx(IntPtr hActCtx, out IntPtr lpCookie);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeactivateActCtx(int dwFlags, IntPtr lpCookie);

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern void ReleaseActCtx(IntPtr hActCtx);


        [DllImport("ole32.dll")]
        public static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]

        public static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);


    }
}