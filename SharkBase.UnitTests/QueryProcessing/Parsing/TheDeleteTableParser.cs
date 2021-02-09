using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.QueryProcessing.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.UnitTests.QueryProcessing.Parsing
{
    [TestClass]
    public class TheDeleteTableParser
    {
        private DeleteTableParser parser;

        [TestInitialize]
        public void Initialize()
        {
            this.parser = new DeleteTableParser();
        }

        [TestMethod]
        public void GivenAnInputStartingWith_DELETE_TABLE_ItIsParsable()
        {
            string input = "DELETE TABLE FOOD";

            Assert.IsTrue(parser.IsParsable(input));
        }

        [TestMethod]
        public void GivenAnInputThatDoesNotStartWith_DELETE_TABLE_ItIsNotParsable()
        {
            Assert.IsFalse(parser.IsParsable("INSERT TABLE"));
        }

        [TestMethod]
        public void RequiresATableName()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("INSERT TABLE"));
        }

        [TestMethod]
        public void ReturnsAStatementWithTheTablesName()
        {
            IStatement statement = parser.Parse("DELETE TABLE FOOD");

            Assert.AreEqual("FOOD", statement.Table);
        }
    }
}
