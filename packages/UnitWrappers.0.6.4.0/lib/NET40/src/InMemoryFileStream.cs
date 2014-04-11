using System.IO;
using UnitWrappers.Microsoft.Win32.SafeHandles;
using UnitWrappers.System.IO;
using UnitWrappers.System.Security.AccessControl;

namespace UnitWrappers
{
    public class InMemoryFileStream : FileStreamBase
    {
        private readonly string _name;
        private Stream _stream;

        public InMemoryFileStream(string name, string data)
        {
            _name = name;
            _stream = generateStreamFromString(data);
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer,offset,count);
        }

        public override bool CanRead { get { return true; } }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override long Position { get { return _stream.Position; } set { _stream.Position = value; } }


        public override void Flush(bool flushToDisk)
        {
            _stream.Flush();
        }


        public override void Lock(long position, long length)
        {
            
        }

        public override void Unlock(long position, long length)
        {
            
        }

        public override bool IsAsync
        {
            get { return false; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override ISafeFileHandle SafeFileHandle
        {
            get { return null; }
        }

        public override IFileSecurity GetAccessControl()
        {
            return null;
        }

        public override void SetAccessControl(IFileSecurity fileSecurity)
        {
            
        }

        private Stream generateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}