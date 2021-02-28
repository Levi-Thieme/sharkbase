using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharkBase.UnitTests.QueryProcessing.Parsing
{
    [TestClass]
    public class TheInsertRecordParser
    {
        private InsertRecordParser parser;
    
        [TestInitialize]
        public void Initialize()
        {
            parser = new InsertRecordParser(new Mock<SchemaRepository>().Object);
        }

        [TestMethod]
        public void GivenAnInputStartingWith_INSERT_INTO_ItIsParsable()
        {
            Assert.IsTrue(parser.IsParsable("INSERT INTO"));
        }

        [TestMethod]
        public void GivenAnInputNotStartingWith_INSERT_INTO_ItIsNotParsable()
        {
            Assert.IsFalse(parser.IsParsable("DELETE TABLE"));
        }

        [TestMethod]
        public void ItRequiresATableName()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("INSERT INTO"));
        }

        [TestMethod]
        public void GivenAValidInput_ItReturnsAStatementWithTheTableName()
        {
            var statement = parser.Parse("INSERT INTO FOOD 1");

            Assert.AreEqual("FOOD", statement.Table);
        }

        [TestMethod]
        public void GivenAValidInput_ItReturnsAStatementWithColumnValues()
        {
            var expected = new List<string> { "1", "Tacos are good.", "30" };

            var statement = parser.Parse("INSERT INTO FOOD 1 'Tacos are good.' 30");

            CollectionAssert.AreEqual(expected, statement.Tokens.ToList());
        }

        [TestMethod]
        public void GivenAValidInputEndingWithASingleQuoteToCloseAStringLiteral_ItReturnsColumnValues()
        {
            var input = new List<string> { "1", "'Tacos'" };
            var expected = new List<string> { "1", "Tacos" };

            var values = parser.GetColumnValues(input.ToArray());

            CollectionAssert.AreEqual(expected, values.ToList());
        }

        [TestMethod]
        public void GivenAStringLiteralIsNotClosedWithASingleQuote_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("INSERT INTO FOOD 1 'TACOS 30"));
        }

        [TestMethod]
        public void GivenAStringLiteralIsMissingAnOpeningSingleQuote_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("INSERT INTO FOOD 1 TACOS' 30"));
        }
    }
}