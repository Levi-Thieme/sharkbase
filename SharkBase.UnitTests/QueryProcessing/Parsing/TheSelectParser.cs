using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.QueryProcessing.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        public void GivenTheInputHasAWhereClause_ItRequiresAtLeastOneSelectionCriteria()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("SELECT FROM FOOD WHERE"));
        }

        [TestMethod]
        public void GivenTheInputHasAWhereClause_ItReturnsAStatementWithTheColumnNameAndValueTokens()
        {
            var expectedTokens = new List<string> { "NAME", "TACOS" };
            var statement = parser.Parse("SELECT FROM FOOD WHERE NAME =   TACOS");

            CollectionAssert.AreEqual(expectedTokens, statement.Tokens.ToList());
        }

        [TestMethod]
        public void GivenTheInputHasAWhereClauseWithNoSpaceBetweenTheEqualSign_ItReturnsAStatementWithTheColumnNameAndValueTokens()
        {
            var expectedTokens = new List<string> { "NAME", "TACOS" };
            var statement = parser.Parse("SELECT FROM FOOD WHERE NAME=TACOS");

            CollectionAssert.AreEqual(expectedTokens, statement.Tokens.ToList());
        }

        [TestMethod]
        public void GivenTheInputHasAWhereClauseWithSpaceOnOneSideOfTheEqualSign_ItReturnsAStatementWithTheColumnNameAndValueTokens()
        {
            var expectedTokens = new List<string> { "NAME", "TACOS" };
            var statement = parser.Parse("SELECT FROM FOOD WHERE NAME= TACOS");

            CollectionAssert.AreEqual(expectedTokens, statement.Tokens.ToList());
        }
    }
}
