using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NRegFreeCom.Interop;

namespace NRegFreeCom
{
    /// <summary>
    /// Makes working with native dlls as with .NET ones. Helps define search paths and transform errors to .NET friendly.
    /// Hides differences of windows versions. 
    /// </summary>
    /// <remarks>
    /// Potential usages - runtime search and invocation of C routines; .NET plugin model extended with native dlls.
    /// Instance methods are not thread safe.
    /// </remarks>
    /// <seealso href="https://gist.github.com/1568627/76da6e65a2d925fffdb48a0ae5efd783281f9244"/>
    /// <seealso href="http://blogs.msdn.com/b/jonathanswift/archive/2006/10/03/dynamically-calling-an-unmanaged-dll-from-.net-_2800_c_23002900_.aspx"/>
    /// <seealso href="http://www.codeproject.com/Articles/1557/Late-binding-on-native-DLLs-with-C"/>
    //TODO: use SafeHandle instead of IntPtr
    //TODO: create UnsafeAssemblySystem which does not integrity checking and path normalization
    //TODO: create SynchrnousAssemblySystem
    public class AssemblySystem : IAssemblySystem, IDisposable
    {
        /// <summary>
        /// Default subdirectory to search 32 bit x86  dlls for <see cref="GetAnyCpuPath"/> .
        /// </summary>
        public string Win32Directory = "Win32";

        /// <summary>
        /// Default subdirectory to search 64 bit x86  dlls for <see cref="GetAnyCpuPath"/> .
        /// </summary>
        public string x64Directory = "x64";

        //NOTE: not sure that using next directory is good for base (may be some native methods are more proper)
        public string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        ///Windows 7, Windows Server 2008 R2, Windows Vista, and Windows Server 2008: 
        ///  To use this function in an application, call GetProcAddress to retrieve the function's address from Kernel32.dll. 
        /// KB2533623 must be installed on the target platform.
        private static Version _systemSupportsPatch = new Version("6.0.6002");
        private bool _hasPatch = true;

        private Dictionary<IntPtr, string> _dirCookies = new Dictionary<IntPtr, string>();
        private List<string> _directories = new List<string>();


        ///<inheritdoc/>
        public string GetAnyCpuPath(string directoryPath)
        {
            //TODO: check not only bits but arch (e.g. ARM or Itanium)
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


        private string makePathRooted(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                return Path.Combine(BaseDirectory, path);
            }
            return path;
        }

        ///<inheritdoc/>
        public IAssembly LoadFrom(string directoryPath, string name)
        {

            string path = Path.Combine(directoryPath, name);
            return LoadFrom(path);
        }

        ///<inheritdoc/>
        public IAssembly LoadFrom(string path)
        {
            path = normalize(path);//fixes problem with dot in paths like "C:/."
            IntPtr hModule;
            if (supportsCustomSearch)
            {
                var flags = LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS |
                            LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR;

                hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, flags);
            }
            else
            {
                hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
            }

            ThrowOnError(path, hModule);
            return new Assembly(hModule, Path.GetFileName(path), path);
        }

        private static void ThrowOnError(string path, IntPtr hModule)
        {
            if (hModule == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                var ex = new Win32Exception(error);
                if (error == SYSTEM_ERROR_CODES.ERROR_MOD_NOT_FOUND
                    || error == SYSTEM_ERROR_CODES.ERROR_ENVVAR_NOT_FOUND) //TODO: change exception - this happens if path not rooted 
                    throw new System.IO.FileNotFoundException("Failed to find dll or its dependencies", path, ex);
                if (error == SYSTEM_ERROR_CODES.ERROR_BAD_EXE_FORMAT
                    || error == SYSTEM_ERROR_CODES.ERROR_INVALID_PARAMETER)//TODO: change exception - this happens if path not rooted 
                    throw new BadImageFormatException("Failed to load dll", path, ex);
                throw ex;
            }
        }


        private string normalize(string path)
        {
            return new Uri(path, UriKind.RelativeOrAbsolute).LocalPath;
        }

        ///<inheritdoc/>
        public void AddSearchPath(string directory)
        {
            if (supportsCustomSearch)
            {
                try
                {
                    directory = normalize(directory);
                    IntPtr cookie = NativeMethods.AddDllDirectory(directory);
                    if (cookie == IntPtr.Zero)
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    _dirCookies.Add(cookie, directory);
                }
                catch (EntryPointNotFoundException ex) // system without patch 
                {
                    Tracing.Source.TraceInformation(string.Format("Failed to AddDllDirectory with next error:{0}", ex));
                    _hasPatch = false;

                    // xp and vista without patch
                    addDllDirectoryToProcessEnvVars(directory);
                    setDllDirectory(directory);
                }
            }
            else
            {
                // xp and vista without patch
                addDllDirectoryToProcessEnvVars(directory);
                setDllDirectory(directory);
            }
        }

        private void setDllDirectory(string directory)
        {
            directory = normalize(directory);
            bool result = NativeMethods.SetDllDirectory(directory);
            if (!result)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }



        // this is last chance usage - security breach because attacker could put its dll in some path
        private void addDllDirectoryToProcessEnvVars(string directory)
        {
            directory = normalize(Path.GetFullPath(directory));
            _directories.Add(directory);//TODO: manage added paths
            try
            {
                var path = getProcessPath();
                var newPath = PathAppender.Append(directory, path);

                if (newPath.Length > path.Length)
                {
                    setProcessPath(newPath); 
                }
                
            }
            catch (Exception ex)
            {
                // consider it OK for for trace analysys (like fusion log analysys)
                Tracing.Source.TraceInformation(string.Format("Failed to add {0} to PATH:{1}", directory, ex));
            }
        }



        private static void setProcessPath(string paths)
        {
            Environment.SetEnvironmentVariable("PATH", paths, EnvironmentVariableTarget.Process);
        }

        private static string getProcessPath()
        {
            var paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            if (String.IsNullOrEmpty(paths))
                paths = string.Empty;
            return paths;
        }

        private bool supportsCustomSearch
        {
            get { return Environment.OSVersion.Version >= _systemSupportsPatch && _hasPatch; }
        }


        public void Dispose()
        {
            foreach (var dirCookie in _dirCookies)
            {
                NativeMethods.RemoveDllDirectory(dirCookie.Key);
            }
            _dirCookies.Clear();

            //TODO: how to remove paths appended cause we can deleted some other paths added?
            _directories.Clear();
            
        }

        public IAssembly ReflectionOnlyLoadFrom(string path)
        {
            path = normalize(path);

            //TODO: check if should use flags that does integrity checking if .NET does such check loading managed dlls for reflection
            //TODO: I think it is .NET design to be safe if possible
            var flags = LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_AS_DATAFILE;

            var hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, flags);
            return new Assembly(hModule, Path.GetFileName(path), path);
        }
    }
}
