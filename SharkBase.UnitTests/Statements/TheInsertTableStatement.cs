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
        private InsertTableStatement statement;

        [TestInitialize]
        public void Initialize()
        {
            validatorMock = new Mock<IStatementValidator>();
            statement = new InsertTableStatement(validatorMock.Object, "test", new List<Column>());
        }

        [TestMethod]
        public void InvokesTheValidator()
        {
            statement.Validate();

            validatorMock.Verify(validator => validator.Validate("test", statement.Tokens), Times.Once);
        }
    }
}
