using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NRegFreeCom
{
    public class AssemblySystem
    {
        public string Win32Directory = "Win32";
        public string x64Directory = "x64";

        public bool AddDllDirectoryToSearchPath { get; set; }

        public Assembly LoadAnyCpuSubLibrary(string name)
        {
            return LoadAnyCpuLibrary(AppDomain.CurrentDomain.BaseDirectory, name);
        }

        public Assembly LoadAnyCpuLibrary(string directoryPath, string name)
        {
            //TODO: check not only bits by arch (e.g. COM on ARM or Itanium)
            IntPtr hModule = IntPtr.Zero;
            string path;
            if (IntPtr.Size == 4)
            {
                var directory = Path.Combine(directoryPath, Win32Directory);
                AddSearchPath(directory);
                path = Path.Combine(directory, name);
                hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
                if (hModule == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else if (IntPtr.Size == 8)
            {
                var directory = Path.Combine(directoryPath, x64Directory);
                AddSearchPath(directory);
                path = Path.Combine(directory, name);
                hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
                if (hModule == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else throw new NotSupportedException("It is 2033 year or some kind of embedded device. Both are not considered.");
            return new Assembly(hModule, name, path);
        }

        private void AddSearchPath(string directory)
        {
            //NOTE: this is thread unsafe hack for Xp and Vista
            //TODO: use Windows 7 features to fix right
            if (AddDllDirectoryToSearchPath)
                NativeMethods.SetDllDirectory(directory);
        }

        public Assembly LoadLibrary(string path)
        {
            AddSearchPath(Directory.GetParent(path).FullName);
            var hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
            //TODO: throws new BadImageFormatException() if manage or not that CPU
            if (hModule == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
            return new Assembly(hModule, Path.GetFileName(path), path);
        }
    }
}
