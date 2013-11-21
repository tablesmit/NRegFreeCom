            
			
			//hook
     
            var CreateFileHook = LocalHook.Create(
               LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
               new DCreateFile(CreateFile_Hooked),
               p);
            CreateFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
			
			            var nativeLibrary = loader.LoadFrom(loader.GetAnyCpuPath(loader.BaseDirectory), "NativeLibrary");
            var registry = nativeLibrary.GetDelegate<ReadRegistry>();
            registry.Invoke();

			
			          [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate IntPtr DCreateFile(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        // just use a P-Invoke implementation to get native API access
        // from C# (this step is not necessary for C++.NET)
        [DllImport("kernel32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr CreateFile(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        // this is where we are intercepting all file accesses!
        static IntPtr CreateFile_Hooked(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile)
        {
            _wasHooked = true;

            return CreateFile(InFileName,
                              InDesiredAccess,
                              InShareMode,
                              InSecurityAttributes,
                              InCreationDisposition,
                              InFlagsAndAttributes,
                              InTemplateFile);
        }
		        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        public delegate int ReadRegistry();

		        public class RegistyEasyHook:EasyHook.IEntryPoint 
        {
            
        }