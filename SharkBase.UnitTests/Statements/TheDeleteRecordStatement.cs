using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.UnitTests.Statements
{
    [TestClass]
    public class TheDeleteRecordStatement
    {
        private Mock<IStatementValidator> mockValidator;
        private DeleteRecordStatement statement;

        [TestInitialize]
        public void Initialize()
        {
            mockValidator = new Mock<IStatementValidator>();
            statement = new DeleteRecordStatement("test", new List<string> { "first", "second" }, mockValidator.Object);
        }

        [TestMethod]
        public void WhenValidating_ItInvokesTheValidator()
        {
            statement.Validate();

            mockValidator.Verify(validator => validator.Validate(statement.Table, statement.Tokens), Times.Once);
        }
    }
}
