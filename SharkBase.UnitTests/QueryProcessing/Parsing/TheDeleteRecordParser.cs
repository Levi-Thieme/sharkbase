using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.UnitTests.QueryProcessing.Parsing
{
    [TestClass]
    public class TheDeleteRecordParser
    {
        private DeleteRecordParser parser;

        [TestInitialize]
        public void Initialize()
        {
            parser = new DeleteRecordParser(new Mock<SchemaRepository>().Object);
        }

        [TestMethod]
        public void GivenInputStartingWith_DELETE_FROM_ItIsParsable()
        {
            Assert.IsTrue(parser.IsParsable("DELETE FROM"));
        }

        [TestMethod]
        public void GivenTheInputDoesNotStartWith_DELETE_FROM_ItIsNotParsable()
        {
            Assert.IsFalse(parser.IsParsable("DELETE INTO"));
        }

        [TestMethod]
        public void ItRequiresATableName()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("DELETE FROM"));
        }

        [TestMethod]
        public void GivenTheInputIsValid_ItReturnsAStatementWithTheTableName()
        {
            var statement = parser.Parse("DELETE FROM FOOD");

            Assert.AreEqual("FOOD", statement.Table);
        }

        [TestMethod]
        public void GivenTheInputHasAWhereClause_ItRequiresAtLeastOneDELETEionCriteria()
        {
            Assert.ThrowsException<ArgumentException>(() => parser.Parse("DELETE FROM FOOD WHERE"));
        }

        [TestMethod]
        public void GivenTheInputHasAWhereClause_ItReturnsAStatementWithTheColumnNameAndValueTokens()
        {
            var expectedTokens = new List<string> { "NAME", "TACOS" };
            var statement = parser.Parse("DELETE FROM FOOD WHERE NAME =   TACOS");

            CollectionAssert.AreEqual(expectedTokens, statement.Tokens.ToList());
        }

        [TestMethod]
        public void GivenTheInputHasAWhereClauseWithNoSpaceBetweenTheEqualSign_ItReturnsAStatementWithTheColumnNameAndValueTokens()
        {
            var expectedTokens = new List<string> { "NAME", "TACOS" };
            var statement = parser.Parse("DELETE FROM FOOD WHERE NAME=TACOS");

            CollectionAssert.AreEqual(expectedTokens, statement.Tokens.ToList());
        }

        [TestMethod]
        public void GivenTheInputHasAWhereClauseWithSpaceOnOneSideOfTheEqualSign_ItReturnsAStatementWithTheColumnNameAndValueTokens()
        {
            var expectedTokens = new List<string> { "NAME", "TACOS" };
            var statement = parser.Parse("DELETE FROM FOOD WHERE NAME= TACOS");

            CollectionAssert.AreEqual(expectedTokens, statement.Tokens.ToList());
        }
    }
}
