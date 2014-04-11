using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitWrappers.System.IO;

namespace UnitWrappers
{
    public class FileSystem
    {
        public FileSystem(IPath path, IFile file, IDirectory directory)
        {
            Directory = directory;
            File = file;
            Path = path;
        }

        public FileSystem():
            this(new PathWrap(), new FileWrap(), new DirectoryWrap()){}

        public IDirectory Directory { get; private set; }
        public IFile File { get; private set; }
        public IPath Path { get; private set; }
        
    }
}
