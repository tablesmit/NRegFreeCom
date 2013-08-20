using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class PEHeaderReaderTests
    {

        [Test]
        public void Pe32BitsLoads64BitsPe()
        {
            var loader = new AssemblySystem();
            //var dll64 = Path.Combine(loader.BaseDirectory, loader.x64Directory, "RegFreeComResources.dll");
            var reader = new BinaryReader(new FileStream(System.Reflection.Assembly.GetExecutingAssembly().Location, FileMode.Open, FileAccess.Read));
            var fileHeader = PeHeaderReader.FromBinaryReader<PeHeaderReader.IMAGE_FILE_HEADER>(reader);
            var imageSectionHeaders = new PeHeaderReader.IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
            for (int headerNo = 0; headerNo < imageSectionHeaders.Length; ++headerNo)
            {
                var section = PeHeaderReader.FromBinaryReader<PeHeaderReader.IMAGE_SECTION_HEADER>(reader);
                Console.WriteLine(new string(section.Name).Replace( "\0", "" ));
            }
        }

    }
}
