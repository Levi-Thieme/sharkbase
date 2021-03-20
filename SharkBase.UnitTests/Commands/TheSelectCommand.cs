using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.DataAccess.Streaming;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System.Collections.Generic;

namespace SharkBase.UnitTests.Commands
{
    [TestClass]
    public class TheSelectCommand
    {
        private Mock<Streamable<Record>> mockRecordStream;
        private Mock<ITable> mockTable;

        [TestInitialize]
        public void Initialize()
        {
            this.mockRecordStream = new Mock<Streamable<Record>>();
            this.mockTable = new Mock<ITable>();
            this.mockTable.Setup(table => table.ReadAll()).Returns(this.mockRecordStream.Object);
        }

        [TestMethod]
        public void WhenExecuted_ItGetsAllRecords()
        {
            var command = new SelectCommand(new SelectStatement(new Mock<IStatementValidator>().Object, "tableName", new List<string>()), mockTable.Object);

            command.Execute();

            mockTable.Verify(table => table.ReadAll(), Times.Once);
        }
    }
}
