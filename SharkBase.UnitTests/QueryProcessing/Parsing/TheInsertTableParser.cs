using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.QueryProcessing.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.UnitTests.QueryProcessing.Parsing
{
    [TestClass]
    public class TheInsertTableParser
    {
        private InsertTableParser parser;

        [TestInitialize]
        public void Initialize()
        {
            this.parser = new InsertTableParser();
        }

        [TestMethod]
        public void GivenAnInputStartingWith_INSERT_TABLE_ItIsParsable()
        {
            string input = "INSERT TABLE FOOD INT64 ID STRING NAME";

            Assert.IsTrue(parser.IsParsable(input));
        }

        [TestMethod]
        public void GivenAnInputThatDoesNotStartWith_INSERT_TABLE_ItIsNotParsable()
        {
            Assert.IsFalse(parser.IsParsable("DELETE TABLE"));
        }

        [TestMethod]
        public void RequiresATableName()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("INSERT TABLE"));
        }

        [TestMethod]
        public void RequiresAtLeastOneColumnDefinition()
        {
            string input = "INSERT TABLE FOOD";

            Assert.ThrowsException<ArgumentException>(() => parser.Parse(input));
        }

        [TestMethod]
        public void ReturnsAStatementWithTheTablesName()
        {
            IStatement statement = parser.Parse("INSERT TABLE FOOD INT64 ID");

            Assert.AreEqual("FOOD", statement.Table);
        }

        [TestMethod]
        public void ReturnsAStatementWithColumnDefinitionTokens()
        {
            var expected = new List<string> { "INT64", "ID", "STRING", "NAME" };

            IStatement statement = parser.Parse("INSERT TABLE FOOD INT64 ID STRING NAME");

            CollectionAssert.AreEqual(expected, statement.Tokens.ToList());
        }
    }
}
