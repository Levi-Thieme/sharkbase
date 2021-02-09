using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Statements;

namespace SharkBase.UnitTests.Commands
{
    [TestClass]
    public class TheDeleteTableCommand
    {
        [TestMethod]
        public void WhenExecuted_ItDeletesTheTableWithTheStatementsTableName()
        {
            var mockTables = new Mock<ITables>();
            var statement = new DeleteTableStatement("tableToDelete", null);
            var command = new DeleteTableCommand(statement, mockTables.Object);

            command.Execute();

            mockTables.Verify(tables => tables.Delete("tableToDelete"), Times.Once);
        }
    }
}
