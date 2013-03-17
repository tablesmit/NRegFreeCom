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
                path = Path.Combine(directory, name);
                hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
                if (hModule == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else if (IntPtr.Size == 8)
            {
                var directory = Path.Combine(directoryPath, x64Directory);
                path = Path.Combine(directory, name);
                hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
                if (hModule == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else throw new NotSupportedException("It is 2033 year or some kind of embedded device. Both are not considered.");
            return new Assembly(hModule, name, path);
        }

        public Assembly LoadLibrary(string path)
        {
            var hModule = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, 0);
            //TODO: throws new BadImageFormatException() if manage or not that CPU
            if (hModule == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
            return new Assembly(hModule, Path.GetFileName(path), path);
        }
    }
}
