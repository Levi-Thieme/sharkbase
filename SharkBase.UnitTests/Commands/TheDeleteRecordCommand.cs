using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.Models;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharkBase.UnitTests.Commands
{
    [TestClass]
    public class TheDeleteRecordCommand
    {
        private DeleteRecordCommand command;
        private Mock<ITable> mockTable;
        private DeleteRecordStatement statement;
        private List<Record> records;

        [TestInitialize]
        public void Initialize()
        {
            statement = new DeleteRecordStatement("test", new List<string> { "ID", "4" }, new Mock<IStatementValidator>().Object);
            var schema = new TableSchema("test", new List<Column> { new Column(ColumnType.Int64, "ID") });
            mockTable = new Mock<ITable>();
            mockTable.SetupGet(table => table.Schema).Returns(schema);
            command = new DeleteRecordCommand(statement, mockTable.Object);
            this.records = new List<Record>
            {
                new Record(new List<Value> { new Value("4") }),
                new Record(new List<Value> { new Value("5") })
            };
            mockTable.Setup(table => table.ReadAllRecords()).Returns(records);
        }

        [TestMethod]
        public void WhenExecuted_ItGetsAllRecords()
        {
            command.Execute();

            mockTable.Verify(table => table.ReadAllRecords(), Times.Once);
        }

        [TestMethod]
        public void WhenExecutedWithNoTokens_ItDeletesAllRecords()
        {
            statement.Tokens = new List<string>();

            command.Execute();

            mockTable.Verify(table => table.DeleteRecords(records), Times.Once);
        }

        [TestMethod]
        public void WhenExecutedWithTokens_ItDeletesRecordsThatMatchTheWhereClause()
        {
            var expectedRecords = records.Where(r => r.Values.First().Equals("4"));

            command.Execute();

            mockTable.Verify(table => table.DeleteRecords(expectedRecords));
        }
    }
}
