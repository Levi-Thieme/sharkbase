using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.QueryProcessing.Parsing;
using SharkBase.QueryProcessing.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.UnitTests.QueryProcessing.Parsing
{
    [TestClass]
    public class TheUpdateRecordParser
    {
        private UpdateRecordParser parser = new UpdateRecordParser();


        [TestMethod]
        public void GivenTheInputDoesNotStartWith_Update_ItIsNotParsable()
        {
            string sql = "Insert into tablename";

            Assert.IsFalse(parser.IsParsable(sql));
        }

        [TestMethod]
        public void GivenTheInputStartsWith_Update_ItIsParsable()
        {
            string sql = "UPDATE scores";

            Assert.IsTrue(parser.IsParsable(sql));
        }

        [TestMethod]
        public void GivenTheInputDoesNotContainATableName_WhenParsing_ItThrowsAnException()
        {
            string sql = "UPDATE";

            Assert.ThrowsException<ArgumentException>(() => parser.Parse(sql));
        }

        [TestMethod]
        public void GivenTheInputDoesNotContainASetColumnStatement_WhenParsing_ItThrowsAnException()
        {
            string sql = "UPDATE food";

            Assert.ThrowsException<ArgumentException>(() => parser.Parse(sql));
        }

        [TestMethod]
        public void GivenTheInputContainsATableNameAndSetColumnStatement_WhenParsing_ItReturnsAnUpdateStatement()
        {
            string sql = "UPDATE food SET price = 5";

            var statement = parser.Parse(sql) as UpdateRecordStatement;

            Assert.AreEqual("food", statement.Table);
            Assert.AreEqual("price", statement.Column);
            Assert.AreEqual("5", statement.Value);
        }

        [TestMethod]
        public void GivenTheInputContainsAWhereClause_WhenParsing_ItReturnsAnUpdateStatementWithTheCorrectWhereColumnAndValue()
        {
            string sql = "UPDATE food SET price = 5 WHERE name = 'tacos'";

            var statement = parser.Parse(sql) as UpdateRecordStatement;

            Assert.AreEqual("name", statement.WhereColumn);
            Assert.AreEqual("tacos", statement.WhereColumnValue);
        }

        [TestMethod]
        public void GivenTheInputDoesNotContainAValidWhereClause_ItThrowsAnException()
        {
            string sql = "UPDATE food SET price = 5 WHERE name =";

            Assert.ThrowsException<ArgumentException>(() => parser.Parse(sql));
        }
    }
}
