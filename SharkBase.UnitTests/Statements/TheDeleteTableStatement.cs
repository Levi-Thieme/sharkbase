using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System.Collections.Generic;

namespace SharkBase.UnitTests.Statements
{
    [TestClass]
    public class TheDeleteTableStatement
    {
        private Mock<IStatementValidator> validatorMock;
        private DeleteTableStatement statement;
        private IEnumerable<string> tokens = new List<string>();

        [TestInitialize]
        public void Initialize()
        {
            validatorMock = new Mock<IStatementValidator>();
            statement = new DeleteTableStatement("test", validatorMock.Object) { Tokens = tokens };
        }

        [TestMethod]
        public void InvokesTheValidator()
        {
            statement.Validate();

            validatorMock.Verify(validator => validator.Validate("test", tokens), Times.Once);
        }
    }
}
