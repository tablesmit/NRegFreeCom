using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NRegFreeCom
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Instance methods are not thread safe.
    /// </remarks>
    public class AssemblySystem
    {
        public string Win32Directory = "Win32";
        public string x64Directory = "x64";
        //NOTE: not sure that using next directoy is good for base (may be some native methods are more proper)
        public string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        ///Windows 7, Windows Server 2008 R2, Windows Vista, and Windows Server 2008: 
        ///  To use this function in an application, call GetProcAddress to retrieve the function's address from Kernel32.dll. 
        /// KB2533623 must be installed on the target platform.
        /// http://support.microsoft.com/kb/2533623
        private static Version _systemSupportsPatch = new Version("6.0.6002");
        private bool _hasPatch = true;

        private List<IntPtr> _dirCookies = new List<IntPtr>();
  

        /// <summary>
        /// Should managed code to look into <see cref="Win32Directory"/> or <see cref="x64Directory"/> for native library suitable process bitness.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public string GetAnyCpuPath(string directoryPath)
        {
            if (IntPtr.Size == 4)
            {
                return Path.Combine(directoryPath, Win32Directory);
            }
            else if (IntPtr.Size == 8)
            {
                return Path.Combine(directoryPath, x64Directory);
            }
            else throw new NotSupportedException("It is 2033 year or some kind of embedded device. Both are not considered.");
        }


        public string MakePathRooted(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                return Path.Combine(BaseDirectory, path);
            }
            return path;
        }


        public Assembly LoadFrom(string directoryPath, string name)
        {
            //TODO: check not only bits by arch (e.g. COM on ARM or Itanium)
            IntPtr hModule = IntPtr.Zero;
            string path;
            var directory = directoryPath;

            path = Path.Combine(directory, name);
            //TODO: throw new BadImageFormatException() if managed or not that CPU
            if (SupportsCustomSearch)
            {
                var flags = LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS |
                            LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR;
                hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, flags);
            }
            else
            {
                hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
            }

            if (hModule == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return new Assembly(hModule, name, path);

        }



        /// <summary>
        /// NOTE: this is  unsafe hack for Xp and Vista. Works well on >= Win7
        /// </summary>
        /// <param name="directory"></param>
        /// <seealso href="http://msdn.microsoft.com/en-us/library/ff919712.aspx"/>
        /// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms682586.aspx"/>
        public void AddSearchPath(string directory)
        {
            if (SupportsCustomSearch)
            {
                IntPtr? cookie = null;
                try
                {
                    cookie = NativeMethods.AddDllDirectory(directory);
                }
                catch (EntryPointNotFoundException ex) // system without patch 
                {
                    Tracing.Source.TraceInformation(string.Format("Failed to AddDllDirectory with next error:{0}", ex));
                    _hasPatch = false;
                    //addDllDirectoryToProcessEnvVars(directory);
                    setDllDirectory(directory);
                }

                if (cookie.HasValue)
                {
                    if (cookie.Value == IntPtr.Zero)
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    _dirCookies.Add(cookie.Value);
                }

            }
            else
            {
                //addDllDirectoryToProcessEnvVars(directory);

                setDllDirectory(directory);
            }
        }

        private static void setDllDirectory(string directory)
        {
            bool result = NativeMethods.SetDllDirectory(directory);
            if (!result)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        // this is last chance usage - security breack because attacker could put its dll in some path
        private static void addDllDirectoryToProcessEnvVars(string directory)
        {
            //TODO: managed added paths
            directory = Path.GetFullPath(directory);
            var path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            if (!path.Contains(directory)) //TODO: normalize pathes before search, what is impact on dulication?
            {
                path += directory + ";";
                Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            }

        }

        private bool SupportsCustomSearch
        {
            get { return Environment.OSVersion.Version >= _systemSupportsPatch && _hasPatch; }
        }


        public Assembly LoadFrom(string path)
        {
            //TODO: throw new BadImageFormatException() if managed or not that CPU
            var hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
            if (hModule == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                var ex = new Win32Exception(error);
                if (error == SYSTEM_ERROR_CODES.ERROR_MOD_NOT_FOUND)
                    throw new System.IO.FileNotFoundException("Failed to find dll", path, ex);
                if (error == SYSTEM_ERROR_CODES.ERROR_BAD_EXE_FORMAT)
                    throw new BadImageFormatException("Failed to load dll", path, ex);
                throw ex;
            }
            return new Assembly(hModule, Path.GetFileName(path), path);
        }
    }
}
