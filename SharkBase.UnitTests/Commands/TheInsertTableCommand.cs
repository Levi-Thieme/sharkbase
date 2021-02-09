using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Validation;
using SharkBase.Statements;
using System.Collections.Generic;

namespace SharkBase.UnitTests.Commands
{
    [TestClass]
    public class TheInsertTableCommand
    {
        [TestMethod]
        public void WhenExecuted_ItInvokesCreateTableWithTheStatementsColumnDefinitions()
        {
            var mockTables = new Mock<ITables>();
            var expectedColumnDefinitions = new List<Column>
            {
                new Column(ColumnType.Int64, "ID"),
                new Column(ColumnType.Char128, "NAME")
            };
            List<string> tokens = new List<string> { "INT64", "ID", "CHAR128", "NAME" };
            var statement = new InsertTableStatement(new Mock<IStatementValidator>().Object) { Table = "PERSON", Tokens = tokens };
            var command = new InsertTableCommand(statement, mockTables.Object);

            command.Execute();

            mockTables.Verify(tables => tables.Create(statement.Table, expectedColumnDefinitions));
        }
    }
}
