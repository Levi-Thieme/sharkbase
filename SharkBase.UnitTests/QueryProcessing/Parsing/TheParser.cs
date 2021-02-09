using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.Parsing;
using System.Collections.Generic;
using SharkBase.QueryProcessing.Parsing;

namespace SharkBase.UnitTests
{
    [TestClass]
    public class TheParser
    {
        private Parser parser;

        [TestInitialize]
        public void Initialize()
        {
            this.parser = new Parser(new List<IParser>());
        }

        [TestMethod]
        public void GivenAnEmptyString_ItDoesNotParseIt()
        {
            Assert.IsFalse(parser.IsParsable(string.Empty));
        }

        [TestMethod]
        public void GivenNull_ItDoesNotParseIt()
        {
            Assert.IsFalse(parser.IsParsable(null));
        }

        [TestMethod]
        public void GivenANonemptyString_ItParsesIt()
        {
            Assert.IsTrue(parser.IsParsable("test"));
        }
    }
}
