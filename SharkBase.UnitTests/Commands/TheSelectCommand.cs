using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Statements;
using System.Collections.Generic;

namespace SharkBase.UnitTests.Commands
{
    [TestClass]
    public class TheSelectCommand
    {
        [TestMethod]
        public void WhenExecuted_ItGetsAllRecords()
        {
            var mockTable = new Mock<ITable>();
            var command = new SelectCommand(new SelectStatement(null, null, new List<string>()), mockTable.Object);

            command.Execute();

            mockTable.Verify(table => table.ReadAllRecords(), Times.Once);
        }
    }
}
