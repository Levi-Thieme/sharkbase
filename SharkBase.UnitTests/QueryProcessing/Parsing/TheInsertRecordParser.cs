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
            var mockSchemas = new Mock<SchemaRepository>();
            var schema = new TableSchema("FOOD", new List<Column> { new Column(ColumnType.String, "NAME", size: 4) });
            mockSchemas.Setup(schemas => schemas.GetSchema("FOOD")).Returns(schema);
            parser = new InsertRecordParser(mockSchemas.Object);
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
            var expected = new List<string> { "test", "30" };

            var statement = parser.Parse("INSERT INTO FOOD 'test' 30");

            CollectionAssert.AreEqual(expected, statement.Tokens.ToList());
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

        [TestMethod]
        public void ItTruncatesStringsToFitInTheirColumn()
        {
            var expected = new List<string> { "taco" };

            var statement = parser.Parse("INSERT INTO FOOD tacos");

            CollectionAssert.AreEqual(expected, statement.Tokens.ToList());

        }

        [TestMethod]
        public void ItPadsStringsToFitInTheirColumn()
        {
            var expected = new List<string> { "ta  " };

            var statement = parser.Parse("INSERT INTO FOOD ta");

            CollectionAssert.AreEqual(expected, statement.Tokens.ToList());
        }
    }
}