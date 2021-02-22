using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.Parsing;
using System;

namespace SharkBase.UnitTests
{
    [TestClass]
    public class TheValueParser
    {
        private ValueParser parser;

        [TestInitialize]
        public void Initialize()
        {
            this.parser = new ValueParser();
        }

        [TestMethod]
        public void ItParsesIntegers()
        {
            Assert.AreEqual(32, parser.ParseInt("32"));
        }

        [TestMethod]
        public void GivenAnInvalidValue_WhenParsingAnInteger_ItThrowsAnException()
        {
            Assert.ThrowsException<FormatException>(() => parser.ParseInt("32.5"));
        }

        [TestMethod]
        public void GivenANull_WhenParsingAString_ItReturnsAnEmptyString()
        {
            Assert.AreEqual(string.Empty, parser.ParseString(null));
        }

        [TestMethod]
        public void ItParsesBools()
        {
            Assert.AreEqual(true, parser.ParseBoolean("true"));
        }
    }
}
