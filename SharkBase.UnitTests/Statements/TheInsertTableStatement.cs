using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Validation;
using SharkBase.Statements;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.UnitTests.Statements
{
    [TestClass]
    public class TheInsertTableStatement
    {
        private Mock<IStatementValidator> validatorMock;
        private List<string> tokens = new List<string> { "INT64", "ID", "STRING", "NAME" };
        private InsertTableStatement statement;

        [TestInitialize]
        public void Initialize()
        {
            validatorMock = new Mock<IStatementValidator>();
            statement = new InsertTableStatement(validatorMock.Object) { Table = "test", Tokens = tokens };
        }

        [TestMethod]
        public void InvokesTheValidator()
        {
            statement.Validate();

            validatorMock.Verify(validator => validator.Validate("test", tokens), Times.Once);
        }

        [TestMethod]
        public void ParsesColumnDefinitions()
        {
            var expectedColumnDefinitions = new List<Column>
            {
                new Column(ColumnType.Int64, "ID"),
                new Column(ColumnType.String, "NAME")
            };
            var statement = new InsertTableStatement(validatorMock.Object) { Table = "test", Tokens = tokens };

            statement.ParseColumnDefinitions();

            CollectionAssert.AreEqual(expectedColumnDefinitions, statement.Columns.ToList());
        }
    }
}
