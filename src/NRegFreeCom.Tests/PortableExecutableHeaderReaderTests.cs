using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class PortableExecutableHeaderReaderTests
    {

        [Test]
        public void Pe32BitsLoads64BitsPe()
        {
            var loader = new AssemblySystem();

            var dll64 = Path.Combine(loader.BaseDirectory, loader.x64Directory, "RegFreeComResources.dll");
            var stream = new FileStream(dll64, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            var current = new PeHeaderReader(stream);

        }

        [Test]
        public void CanReadCustomStringSection()
        {
            var loader = new AssemblySystem();

            var dll64 = Path.Combine(loader.BaseDirectory, loader.x64Directory, "RegFreeComResources.dll");
            var stream = new FileStream(dll64, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            var current = new PeHeaderReader(stream);

            var actual = new List<PeHeaderReader.IMAGE_SECTION_HEADER>();

            for (int headerNo = 0; headerNo < current.ImageSectionHeaders.Length; ++headerNo)
            {
                PeHeaderReader.IMAGE_SECTION_HEADER section = current.ImageSectionHeaders[headerNo];
               actual.Add(section);
            }
            var customSection = actual.Single(x => new String(x.Name).StartsWith(".my_str"));
            var str1 = "MYSTR";
            stream.Seek(customSection.PointerToRawData + str1.Length, SeekOrigin.Begin);
            var reader = new BinaryReader(stream);
            var customData  = reader.ReadChars((int)customSection.SizeOfRawData).Where(x=> x != '\0').ToArray();
            var str = new string(customData);
            StringAssert.Contains(str, "My name is My name is My name is My name name is Last*");
        }

        [Test]
        public void Current()
        {
            var current = new PeHeaderReader(typeof(PortableExecutableHeaderReaderTests).Assembly.Location);
            var expected = new List<string> { ".text", ".rsrs", ".reloc" };
            var actual = new List<string>();

            for (int headerNo = 0; headerNo < current.ImageSectionHeaders.Length; ++headerNo)
            {
                PeHeaderReader.IMAGE_SECTION_HEADER section = current.ImageSectionHeaders[headerNo];
               actual.Add(new string(section.Name));
            }
            Assert.AreEqual(expected.Count,actual.Count);
        }

    }


}
