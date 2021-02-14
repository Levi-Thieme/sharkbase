using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System.Collections.Generic;

namespace SharkBase.UnitTests.Statements
{
    [TestClass]
    public class TheSelectStatement
    {
        [TestMethod]
        public void WhenValidating_ItInvokesTheValidator()
        {
            var validatorMock = new Mock<IStatementValidator>();
            var statement = new SelectStatement(validatorMock.Object, "test", new List<string>());

            statement.Validate();

            validatorMock.Verify(validator => validator.Validate(statement.Table, statement.Tokens), Times.Once);
        }
    }
}
