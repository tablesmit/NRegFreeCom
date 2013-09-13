using System;
using NUnit.Framework;

namespace NRegFreeCom.Tests
{
    [TestFixture]
    public class PathAppenderTests
    {
        [Test]
        public void Append_delimeterVariance_allTheSame()
        {
            var noDelimeterOnEnd = @"C:\a;C:\b\";
            var delimeterOnEnd = @"C:\a;C:\b\;";

            var add = @"C:\c";

            var expected = @"C:\a;C:\b\;C:\c;";

            Assert.AreEqual(expected, PathAppender.Append(noDelimeterOnEnd, add));
            Assert.AreEqual(expected, PathAppender.Append(delimeterOnEnd, add));
    

        }


    }
}