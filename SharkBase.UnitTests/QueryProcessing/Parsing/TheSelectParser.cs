using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.QueryProcessing.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.UnitTests.QueryProcessing.Parsing
{
    [TestClass]
    public class TheSelectParser
    {
        private SelectParser parser;

        [TestInitialize]
        public void Initialize()
        {
            this.parser = new SelectParser();
        }

        [TestMethod]
        public void GivenTheInputDoesNotStartWithSelectFrom_ItIsNotParsable()
        {
            Assert.IsFalse(parser.IsParsable("SELECT INTO"));
        }

        [TestMethod]
        public void GivenTheInputStartsWithSelectFrom_ItIsParsable()
        {
            Assert.IsTrue(parser.IsParsable("SELECT FROM"));
        }

        [TestMethod]
        public void GivenTheInputStartsWithSelectFrom_ItRequiresATableNameWhenParsing()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("SELECT FROM"));
        }

        [TestMethod]
        public void GivenTheInputIsValid_ItReturnsAStatementWithTheTableName()
        {
            var statement = parser.Parse("SELECT FROM FOOD");

            Assert.AreEqual("FOOD", statement.Table);
        }
    }
}
