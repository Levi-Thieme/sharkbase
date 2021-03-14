using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Parsing;
using SharkBase.Statements;
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
            string input = "INSERT TABLE FOOD ID INT 64";

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
            IStatement statement = parser.Parse("INSERT TABLE FOOD ID INT64");

            Assert.AreEqual("FOOD", statement.Table);
        }

        [TestMethod]
        public void ReturnsAStatementWithColumnDefinitionTokens()
        {
            var expected = new List<Column>
            {
                new Column(ColumnType.Int64, "ID"),
                new Column(ColumnType.String, "NAME", size: 48)
            };

            var insertStatement = parser.Parse("INSERT TABLE FOOD ID INT64 NAME STRING(48)") as InsertTableStatement;

            CollectionAssert.AreEqual(expected, insertStatement.Columns.ToList());
        }

        [TestMethod]
        public void GivenAColumnDefinitionOfStringWithoutALengthSpecified_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("INSERT TABLE FOOD NAME STRING()"));
        }
    }
}
