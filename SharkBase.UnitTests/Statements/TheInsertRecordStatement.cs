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
    public class TheInsertRecordStatement
    {
        private Mock<IStatementValidator> mockValidator;
        private InsertRecordStatement statement;
        private List<string> tokens = new List<string> { "hi", "bye" };

        [TestInitialize]
        public void Initialize()
        {
            mockValidator = new Mock<IStatementValidator>();
            statement = new InsertRecordStatement("table", tokens, mockValidator.Object);
        }

        [TestMethod]
        public void WhenValidating_ItInvokesTheValidator()
        {
            statement.Validate();

            mockValidator.Verify(validator => validator.Validate("table", tokens), Times.Once);
        }
    }
}
