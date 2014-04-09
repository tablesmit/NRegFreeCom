using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class RegFileReaderTests
    {

        [Test]
        public void Create_emptyFile_zeroKeys()
        {
            var stream = new MemoryStream();
            var reader = new RegFileReader(stream);
            Assert.AreEqual(0,reader.RegValues.Count);
        }

        [Test]
        public void Create_regedit5WithOneKey_OK()
        {
            var stream = stringToStream(
@"Windows Registry Editor Version 5.00
[HKEY_LOCAL_MACHINE\SOFTWARE\MyKey]",
  Encoding.UTF8);
            var reader = new RegFileReader(stream);
            Assert.AreEqual(1, reader.RegValues.Count);
            Assert.IsTrue(reader.RegValues.ContainsKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\MyKey"));
        }

        [Test]
        public void Create_oneKeyWithOneValue_OK()
        {
            var text = 
@"Windows Registry Editor Version 5.00
[HKEY_LOCAL_MACHINE\SOFTWARE\MyKey]
" +
"\"MyValue\"=\"_RegFreeComRotClass\"";
            var stream = stringToStream(text,Encoding.UTF8);
            var reader = new RegFileReader(stream);
            var key = reader.RegValues[@"HKEY_LOCAL_MACHINE\SOFTWARE\MyKey"];
            Assert.IsNotNull(key);
            Assert.IsTrue(key.ContainsKey("MyValue"));
        }

        public Stream stringToStream(string s,Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, encoding);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

    }
}
