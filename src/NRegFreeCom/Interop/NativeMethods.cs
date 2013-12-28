using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using NRegFreeCom.Interop.ComTypes;

namespace NRegFreeCom.Interop
{
    [SuppressUnmanagedCodeSecurity]
    public static class NativeMethods
    {
        ///<summary>
        ///http://msdn.microsoft.com/en-us/library/windows/desktop/ms678485.aspx
        ///                HRESULT OleLoadPicturePath(
        ///  _In_   LPOLESTR szURLorPath,
        ///  _In_   LPUNKNOWN punkCaller,
        ///  _In_   DWORD dwReserved,
        ///  _In_   OLE_COLOR clrReserved,
        ///  _In_   REFIID riid,
        ///  _Out_  LPVOID *ppvRet
        ///);
        ///   </summary>
        [DllImport("oleaut32.dll")]
        public static extern int OleLoadPicturePath(
             string szURLorPath,
             IntPtr punkCaller,
             uint dwReserved,
             uint clrReserved,
            ref Guid riid,
             out IUnknown unknown);

        [DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern void CoCreateInstance(
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
           [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
           CLSCTX dwClsContext,
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
           [MarshalAs(UnmanagedType.IUnknown)] out object rReturnedComObject);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern ushort RegisterClass([In] ref WNDCLASS lpWndClass);

        /// <summary>
        /// Unregisters a window class, freeing the memory required for the class.
        /// </summary>
        /// <param name="lpClassName">
        /// Type: LPCTSTR
        /// A null-terminated string or a class atom. If lpClassName is a string, it specifies the window class name. 
        /// This class name must have been registered by a previous call to the RegisterClass or RegisterClassEx function. 
        /// System classes, such as dialog box controls, cannot be unregistered. If this parameter is an atom, 
        ///   it must be a class atom created by a previous call to the RegisterClass or RegisterClassEx function. 
        /// The atom must be in the low-order word of lpClassName; the high-order word must be zero.
        /// 
        /// </param>
        /// <param name="hInstance">
        /// A handle to the instance of the module that created the class.
        /// 
        /// </param>
        /// <returns>
        /// Type: BOOL
        /// If the function succeeds, the return value is nonzero.
        /// If the class could not be found or if a window still exists that was created with the class, the return value is zero. 
        /// To get extended error information, call GetLastError.
        /// 
        /// </returns>
        [DllImport("user32.dll")]
        public static extern bool UnregisterClass(IntPtr lpClassName, IntPtr hInstance);

        [DllImport("ole32.dll")]
        public static extern int CoRegisterMessageFilter(
            IMessageFilter lpMessageFilter,
            out IMessageFilter lplpMessageFilter);

        [DllImport("ole32.dll")]
        public static extern int CoMarshalInterThreadInterfaceInStream([In] ref Guid riid,
           [MarshalAs(UnmanagedType.IUnknown)] object pUnk, out IStream ppStm);

        [DllImport("ole32.dll")]
        public static extern int CoGetInterfaceAndReleaseStream(IStream pStm, [In] ref
   Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, StringBuilder lpFilename, int nSize);


        /// <summary>
        /// The CreateWindowEx function creates an overlapped, pop-up, or child window with an extended window style; otherwise, this function is identical to the CreateWindow function. 
        /// </summary>
        /// <param name="dwExStyle">Specifies the extended window style of the window being created.</param>
        /// <param name="lpClassName">Pointer to a null-terminated string or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the high-order word must be zero. If lpClassName is a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, provided that the module that registers the class is also the module that creates the window. The class name can also be any of the predefined system class names.</param>
        /// <param name="lpWindowName">Pointer to a null-terminated string that specifies the window name. If the window style specifies a title bar, the window title pointed to by lpWindowName is displayed in the title bar. When using CreateWindow to create controls, such as buttons, check boxes, and static controls, use lpWindowName to specify the text of the control. When creating a static control with the SS_ICON style, use lpWindowName to specify the icon name or identifier. To specify an identifier, use the syntax "#num". </param>
        /// <param name="dwStyle">Specifies the style of the window being created. This parameter can be a combination of window styles, plus the control styles indicated in the Remarks section.</param>
        /// <param name="x">Specifies the initial horizontal position of the window. For an overlapped or pop-up window, the x parameter is the initial x-coordinate of the window's upper-left corner, in screen coordinates. For a child window, x is the x-coordinate of the upper-left corner of the window relative to the upper-left corner of the parent window's client area. If x is set to CW_USEDEFAULT, the system selects the default position for the window's upper-left corner and ignores the y parameter. CW_USEDEFAULT is valid only for overlapped windows; if it is specified for a pop-up or child window, the x and y parameters are set to zero.</param>
        /// <param name="y">Specifies the initial vertical position of the window. For an overlapped or pop-up window, the y parameter is the initial y-coordinate of the window's upper-left corner, in screen coordinates. For a child window, y is the initial y-coordinate of the upper-left corner of the child window relative to the upper-left corner of the parent window's client area. For a list box y is the initial y-coordinate of the upper-left corner of the list box's client area relative to the upper-left corner of the parent window's client area.
        /// <para>If an overlapped window is created with the WS_VISIBLE style bit set and the x parameter is set to CW_USEDEFAULT, then the y parameter determines how the window is shown. If the y parameter is CW_USEDEFAULT, then the window manager calls ShowWindow with the SW_SHOW flag after the window has been created. If the y parameter is some other value, then the window manager calls ShowWindow with that value as the nCmdShow parameter.</para></param>
        /// <param name="nWidth">Specifies the width, in device units, of the window. For overlapped windows, nWidth is the window's width, in screen coordinates, or CW_USEDEFAULT. If nWidth is CW_USEDEFAULT, the system selects a default width and height for the window; the default width extends from the initial x-coordinates to the right edge of the screen; the default height extends from the initial y-coordinate to the top of the icon area. CW_USEDEFAULT is valid only for overlapped windows; if CW_USEDEFAULT is specified for a pop-up or child window, the nWidth and nHeight parameter are set to zero.</param>
        /// <param name="nHeight">Specifies the height, in device units, of the window. For overlapped windows, nHeight is the window's height, in screen coordinates. If the nWidth parameter is set to CW_USEDEFAULT, the system ignores nHeight.</param> <param name="hWndParent">Handle to the parent or owner window of the window being created. To create a child window or an owned window, supply a valid window handle. This parameter is optional for pop-up windows.
        /// <para>Windows 2000/XP: To create a message-only window, supply HWND_MESSAGE or a handle to an existing message-only window.</para></param>
        /// <param name="hMenu">Handle to a menu, or specifies a child-window identifier, depending on the window style. For an overlapped or pop-up window, hMenu identifies the menu to be used with the window; it can be NULL if the class menu is to be used. For a child window, hMenu specifies the child-window identifier, an integer value used by a dialog box control to notify its parent about events. The application determines the child-window identifier; it must be unique for all child windows with the same parent window.</param>
        /// <param name="hInstance">Handle to the instance of the module to be associated with the window.</param> <param name="lpParam">Pointer to a value to be passed to the window through the CREATESTRUCT structure (lpCreateParams member) pointed to by the lParam param of the WM_CREATE message. This message is sent to the created window by this function before it returns.
        /// <para>If an application calls CreateWindow to create a MDI client window, lpParam should point to a CLIENTCREATESTRUCT structure. If an MDI client window calls CreateWindow to create an MDI child window, lpParam should point to a MDICREATESTRUCT structure. lpParam may be NULL if no additional data is needed.</para></param>
        /// <returns>If the function succeeds, the return value is a handle to the new window.
        /// <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
        /// <para>This function typically fails for one of the following reasons:</para>
        /// <list type="">
        /// <item>an invalid parameter value</item>
        /// <item>the system class was registered by a different module</item>
        /// <item>The WH_CBT hook is installed and returns a failure code</item>
        /// <item>if one of the controls in the dialog template is not registered, or its window window procedure fails WM_CREATE or WM_NCCREATE</item>
        /// </list></returns>

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
           WindowStylesEx dwExStyle,
           IntPtr lpClassName,
           string lpWindowName,
           WindowStyles dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);


        [DllImport("kernel32.dll", SetLastError = true)]
        [PreserveSig]
        public static extern uint GetModuleFileName
        (
            [In]
    IntPtr hModule,

            [Out] 
    StringBuilder lpFilename,

            [In]
    [MarshalAs(UnmanagedType.U4)]
    int nSize
);

        ///<summary>
        ///        DWORD WINAPI GetDllDirectory(
        ///  _In_   DWORD nBufferLength,
        ///  _Out_  LPTSTR lpBuffer
        ///);
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetDllDirectory(int nBufferLength, StringBuilder lpBuffer);

        ///<summary>
        ///        DLL_DIRECTORY_COOKIE  WINAPI AddDllDirectory(
        ///  _In_  PCWSTR NewDirectory
        ///);
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr AddDllDirectory(string NewDirectory);



        ///<summary>
        ///         BOOL  WINAPI SetDefaultDllDirectories(
        ///  _In_  DWORD DirectoryFlags
        ///);
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultDllDirectories(DIRECTORY_FLAGS DirectoryFlags);

        ///<summary>
        ///       BOOL  WINAPI RemoveDllDirectory(
        ///_In_  DLL_DIRECTORY_COOKIE Cookie
        ///);
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RemoveDllDirectory(IntPtr Cookie);

        ///<seealso href="http://search.microsoft.com/en-US/results.aspx?q=SetDllDirectory"/>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(string lpPathName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr FindResource(IntPtr hModule, uint lpName, RESOURCE_TYPES lpType);



        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", SetLastError = true)]
        public static extern SafeLibraryHandle LoadLibrary_Marshaled([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);



        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

   

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true)]
        public static extern bool FreeLibrary(int hModule);

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern object CoGetClassObject(
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
           CLSCTX dwClsContext,
           IntPtr pServerInfo,
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <param name="hFile"></param>
        /// <param name="dwFlags">The action to be taken when loading the module. If no flags are specified, the behavior of this function is identical to that of the LoadLibrary function. This parameter can be one of the following values.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, LOAD_LIBRARY_FLAGS dwFlags);

        [DllImport("kernel32.dll", SetLastError = true,EntryPoint = "LoadLibraryEx")]
        public static extern SafeLibraryHandle LoadLibraryEx_Marshaled(string lpFileName, IntPtr hFile, LOAD_LIBRARY_FLAGS dwFlags);
        

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);


        [DllImport("kernel32.dll", EntryPoint = "EnumResourceNamesW", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumResourceNames(
          IntPtr hModule,
          uint lpszType,
          ENUMRESNAMEPROC lpEnumFunc,
          IntPtr lParam);






        /// <summary>Initializes the COM library for use by the calling thread, sets the thread's concurrency model, and creates a new apartment for the thread if one is required.</summary>
        /// <param name="pvReserved">This parameter is reserved and must be NULL.</param>
        /// <param name="coInit">The concurrency model and initialization options for the thread. Values for this parameter are taken from the CoInit enumeration. Any combination of values can be used, except that the ApartmentThreaded and MultiThreaded flags cannot both be set. The default is MultiThreaded.</param>
        /// <returns>If function succeeds, it returns S_OK. Otherwise, it returns an error code.</returns>
        [DllImport("ole32.dll")]
        public static extern int CoInitializeEx(IntPtr pvReserved, CoInit coInit);

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
        /// <para>The DestroyWindow function destroys the specified window. The function sends WM_DESTROY and WM_NCDESTROY messages to the window to deactivate it and remove the keyboard focus from it. The function also destroys the window's menu, flushes the thread message queue, destroys timers, removes clipboard ownership, and breaks the clipboard viewer chain (if the window is at the top of the viewer chain).</para>
        /// <para>If the specified window is a parent or owner window, DestroyWindow automatically destroys the associated child or owned windows when it destroys the parent or owner window. The function first destroys child or owned windows, and then it destroys the parent or owner window.</para>
        /// <para>DestroyWindow also destroys modeless dialog boxes created by the CreateDialog function.</para>
        /// </summary>
        /// <param name="hwnd">Handle to the window to be destroyed.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);


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
        public static extern int CreatePointerMoniker([MarshalAs(UnmanagedType.IUnknown)] object
           punk, out System.Runtime.InteropServices.ComTypes.IMoniker ppmk);


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