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
using SharkBase.Models.Values;
using SharkBase.DataAccess.Streaming;

namespace SharkBase.UnitTests.Commands
{
    [TestClass]
    public class TheDeleteRecordCommand
    {
        private DeleteRecordCommand command;
        private Mock<ITable> mockTable;
        private DeleteRecordStatement statement;
        private List<Record> records;
        private Mock<Streamable<Record>> mockRecordStream;

        [TestInitialize]
        public void Initialize()
        {
            statement = new DeleteRecordStatement("test", new List<string> { "ID", "4" }, new Mock<IStatementValidator>().Object);
            var schema = new TableSchema("test", new List<Column> { new Column(DataTypes.String, "ID") });
            mockTable = new Mock<ITable>();
            mockTable.SetupGet(table => table.Schema).Returns(schema);
            command = new DeleteRecordCommand(statement, mockTable.Object);
            this.records = new List<Record>
            {
                new Record(new List<Value> { new StringValue("4") }),
                new Record(new List<Value> { new StringValue("5") })
            };
            mockRecordStream = new Mock<Streamable<Record>>();
            mockTable.Setup(table => table.ReadAllRecords()).Returns(records);
            mockTable.Setup(table => table.ReadAll()).Returns(mockRecordStream.Object);
            mockRecordStream.Setup(stream => stream.GetEnumerator()).Returns(this.records.GetEnumerator());
        }

        [TestMethod]
        public void WhenExecuted_ItGetsAllRecords()
        {
            command.Execute();

            mockTable.Verify(table => table.ReadAll(), Times.Once);
        }

        [TestMethod]
        public void WhenExecutedWithNoTokens_ItDeletesAllRecords()
        {
            statement.Tokens = new List<string>();

            command.Execute();

            mockTable.Verify(table => table.DeleteAllRecords(), Times.Once);
        }

        [TestMethod]
        public void WhenExecutedWithTokens_ItDeletesRecordsThatMatchTheWhereClause()
        {
            var expectedRecord = records.First(r => r.Values.First().Equals(new StringValue("4")));

            command.Execute();

            mockTable.Verify(table => table.DeleteRecord(expectedRecord));
        }
    }
}
