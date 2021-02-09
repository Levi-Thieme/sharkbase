using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.UnitTests.Commands
{
    [TestClass]
    public class TheInsertRecordCommand
    {
        private Mock<ITable> table;
        private Mock<IValueParser> parser;
        private InsertRecordStatement statement;
        private InsertRecordCommand command;
        private TableSchema schema;

        [TestInitialize]
        public void Initialize()
        {
            table = new Mock<ITable>();
            parser = new Mock<IValueParser>();
            statement = new InsertRecordStatement("tableName", new List<string>(), new Mock<IStatementValidator>().Object);
            schema = new TableSchema("tableName", new List<Column>());
            command = new InsertRecordCommand(statement, schema, table.Object, parser.Object);
        }

        [TestMethod]
        public void WhenExecuted_ItParsesTheColumnValues()
        {
            command.Execute();

            parser.Verify(p => p.ParseColumnValues(statement.ColumnValues, schema.Columns), Times.Once);
        }
    }
}
