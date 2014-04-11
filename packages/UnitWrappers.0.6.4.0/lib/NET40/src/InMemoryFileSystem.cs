using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnitWrappers.Microsoft.Win32.SafeHandles;
using UnitWrappers.System.IO;
using UnitWrappers.System.Security.AccessControl;

namespace UnitWrappers
{


    public class InMemoryFileSystem : IDirectory, IPath,IFile
    {
    	private class Node
    	{


    		public Node(string name, FileAttributes attrs)
    		{
    			Attributes = attrs;
    			Name = name;
    			Parents = new HashSet<Node>();
    			Contains = new HashSet<Node>();
    		}

    		public string Name { get; set; }
    		public FileAttributes Attributes { get; set; }
    		public HashSet<Node> Parents { get; set; }
    		public HashSet<Node> Contains { get; set; }

    		public void AddContent(Node node)
    		{
    			Contains.Add(node);
    		}



    		public override int GetHashCode()
    		{
    			return (Name != null ? Name.GetHashCode() : 0);
    		}
    	}
    	
        private IPath p = new PathWrap();
        private Node root = new Node("mem", FileAttributes.Device);


        IDirectoryInfo IDirectory.CreateDirectory(string path)
        {
            var driveSeparator = path.IndexOf(p.VolumeSeparatorChar);
            var driveName = path.Remove(driveSeparator);
            var start = new Node(driveName, FileAttributes.Device);
            root.AddContent(start);
            var dirs = path.Substring(driveSeparator + 2).Split(p.AltDirectorySeparatorChar);

            foreach (var dir in dirs)
            {
                var child = new Node(dir, FileAttributes.Directory);            
                start.AddContent(child);
                start = child;
            }
            return null;
        }

        IDirectoryInfo IDirectory.CreateDirectory(string path, UnitWrappers.System.Security.AccessControl.IDirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        void IDirectory.Delete(string path)
        {
            throw new NotImplementedException();
        }

        void IDirectory.Delete(string path, bool recursive)
        {
            throw new NotImplementedException();
        }

        bool IDirectory.Exists(string path)
        {
            var driveSeparator = path.IndexOf(p.VolumeSeparatorChar);
            var driveName = path.Remove(driveSeparator);
            Node start = root.Contains.Where(x => x.Name == driveName).FirstOrDefault() ;
            if (start == null) return false;
            var dirs = path.Substring(driveSeparator + 2).Split(p.AltDirectorySeparatorChar);
            foreach (var dir in dirs)
            {
                Node child = start.Contains.Where(x => x.Name == dir).FirstOrDefault();
                if (child == null) return false;
                start = child;
            }
            return true;
        }

        UnitWrappers.System.Security.AccessControl.IDirectorySecurity IDirectory.GetAccessControl(string path)
        {
            throw new NotImplementedException();
        }

        UnitWrappers.System.Security.AccessControl.IDirectorySecurity IDirectory.GetAccessControl(string path, global::System.Security.AccessControl.AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        DateTime IDirectory.GetCreationTime(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IDirectory.GetCreationTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        string IDirectory.GetCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetDirectories(string path)
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetDirectories(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetDirectories(string path, string searchPattern, global::System.IO.SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        string IDirectory.GetDirectoryRoot(string path)
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetFiles(string path)
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetFiles(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetFiles(string path, string searchPattern, global::System.IO.SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetFileSystemEntries(string path)
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetFileSystemEntries(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        DateTime IDirectory.GetLastAccessTime(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IDirectory.GetLastAccessTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IDirectory.GetLastWriteTime(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IDirectory.GetLastWriteTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        string[] IDirectory.GetLogicalDrives()
        {
            throw new NotImplementedException();
        }

        IDirectoryInfo IDirectory.GetParent(string path)
        {
            throw new NotImplementedException();
        }

        void IDirectory.Move(string sourceDirName, string destDirName)
        {
            throw new NotImplementedException();
        }

        void IDirectory.SetAccessControl(string path, UnitWrappers.System.Security.AccessControl.IDirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        void IDirectory.SetCreationTime(string path, DateTime creationTime)
        {
            throw new NotImplementedException();
        }

        void IDirectory.SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            throw new NotImplementedException();
        }

        void IDirectory.SetCurrentDirectory(string path)
        {
            throw new NotImplementedException();
        }

        void IDirectory.SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            throw new NotImplementedException();
        }

        void IDirectory.SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            throw new NotImplementedException();
        }

        void IDirectory.SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            throw new NotImplementedException();
        }

        void IDirectory.SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            throw new NotImplementedException();
        }

        char IPath.AltDirectorySeparatorChar
        {
            get { throw new NotImplementedException(); }
        }

        char IPath.DirectorySeparatorChar
        {
            get { throw new NotImplementedException(); }
        }

        char IPath.PathSeparator
        {
            get { throw new NotImplementedException(); }
        }

        char IPath.VolumeSeparatorChar
        {
            get { throw new NotImplementedException(); }
        }

        string IPath.ChangeExtension(string path, string extension)
        {
            throw new NotImplementedException();
        }

        string IPath.Combine(string path1, string path2)
        {
            throw new NotImplementedException();
        }

        string IPath.Combine(string path1, string path2, string path3)
        {
            throw new NotImplementedException();
        }

        string IPath.Combine(string path1, string path2, string path3, string path4)
        {
            throw new NotImplementedException();
        }

        string IPath.Combine(params string[] paths)
        {
            throw new NotImplementedException();
        }

        string IPath.GetDirectoryName(string path)
        {
            throw new NotImplementedException();
        }

        string IPath.GetExtension(string path)
        {
            throw new NotImplementedException();
        }

        string IPath.GetFileName(string path)
        {
            throw new NotImplementedException();
        }

        string IPath.GetFileNameWithoutExtension(string path)
        {
            throw new NotImplementedException();
        }

        string IPath.GetFullPath(string path)
        {
            throw new NotImplementedException();
        }

        char[] IPath.GetInvalidFileNameChars()
        {
            throw new NotImplementedException();
        }

        char[] IPath.GetInvalidPathChars()
        {
            throw new NotImplementedException();
        }

        string IPath.GetPathRoot(string path)
        {
            throw new NotImplementedException();
        }

        string IPath.GetRandomFileName()
        {
            throw new NotImplementedException();
        }

        string IPath.GetTempFileName()
        {
            throw new NotImplementedException();
        }

        string IPath.GetTempPath()
        {
            throw new NotImplementedException();
        }

        bool IPath.HasExtension(string path)
        {
            throw new NotImplementedException();
        }

        bool IPath.IsPathRooted(string path)
        {
            throw new NotImplementedException();
        }

        void IFile.AppendAllText(string path, string contents)
        {
            throw new NotImplementedException();
        }

        void IFile.AppendAllText(string path, string contents, global::System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }

        StreamWriterBase IFile.AppendText(string path)
        {
            throw new NotImplementedException();
        }

        void IFile.Copy(string sourceFileName, string destFileName)
        {
            throw new NotImplementedException();
        }

        void IFile.Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.Create(string path)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.Create(string path, int bufferSize)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.Create(string path, int bufferSize, FileOptions options)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.Create(string path, int bufferSize, FileOptions options, UnitWrappers.System.Security.AccessControl.IFileSecurity fileSecurity)
        {
            throw new NotImplementedException();
        }

        StreamWriterBase IFile.CreateText(string path)
        {
            throw new NotImplementedException();
        }

        void IFile.Decrypt(string path)
        {
            throw new NotImplementedException();
        }

        void IFile.Delete(string path)
        {
            throw new NotImplementedException();
        }

        void IFile.Encrypt(string path)
        {
            throw new NotImplementedException();
        }

        bool IFile.Exists(string path)
        {
            throw new NotImplementedException();
        }

        UnitWrappers.System.Security.AccessControl.IFileSecurity IFile.GetAccessControl(string path)
        {
            throw new NotImplementedException();
        }

        UnitWrappers.System.Security.AccessControl.IFileSecurity IFile.GetAccessControl(string path, global::System.Security.AccessControl.AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        FileAttributes IFile.GetAttributes(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IFile.GetCreationTime(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IFile.GetCreationTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IFile.GetLastAccessTime(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IFile.GetLastAccessTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IFile.GetLastWriteTime(string path)
        {
            throw new NotImplementedException();
        }

        DateTime IFile.GetLastWriteTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        void IFile.Move(string sourceFileName, string destFileName)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.Open(string path, FileMode mode)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.Open(string path, FileMode mode, FileAccess access)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.OpenRead(string path)
        {
            throw new NotImplementedException();
        }

        StreamReaderBase IFile.OpenText(string path)
        {
            throw new NotImplementedException();
        }

        FileStreamBase IFile.OpenWrite(string path)
        {
            throw new NotImplementedException();
        }

        byte[] IFile.ReadAllBytes(string path)
        {
            throw new NotImplementedException();
        }

        string[] IFile.ReadAllLines(string path)
        {
            throw new NotImplementedException();
        }

        string[] IFile.ReadAllLines(string path, global::System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }

        string IFile.ReadAllText(string path)
        {
            throw new NotImplementedException();
        }

        string IFile.ReadAllText(string path, global::System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }

        void IFile.Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            throw new NotImplementedException();
        }

        void IFile.Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException();
        }

        void IFile.SetAccessControl(string path, UnitWrappers.System.Security.AccessControl.IFileSecurity fileSecurity)
        {
            throw new NotImplementedException();
        }

        void IFile.SetAttributes(string path, FileAttributes fileAttributes)
        {
            throw new NotImplementedException();
        }

        void IFile.SetCreationTime(string path, DateTime creationTime)
        {
            throw new NotImplementedException();
        }

        void IFile.SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            throw new NotImplementedException();
        }

        void IFile.SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            throw new NotImplementedException();
        }

        void IFile.SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            throw new NotImplementedException();
        }

        void IFile.SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            throw new NotImplementedException();
        }

        void IFile.SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            throw new NotImplementedException();
        }

        void IFile.WriteAllBytes(string path, byte[] bytes)
        {
           
        }

        void IFile.WriteAllLines(string path, string[] contents)
        {
            throw new NotImplementedException();
        }

        void IFile.WriteAllLines(string path, string[] contents, global::System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }

        void IFile.WriteAllText(string path, string contents)
        {
            throw new NotImplementedException();
        }

        void IFile.WriteAllText(string path, string contents, global::System.Text.Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}
