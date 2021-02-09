using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Statements;
using SharkBase.Statements;
using System;

namespace SharkBase.UnitTests.Commands
{
    [TestClass]
    public class TheCommandBuilder
    {
        private CommandBuilder builder;

        [TestInitialize]
        public void Initialize()
        {
            this.builder = new CommandBuilder(new Mock<ITables>().Object, new Mock<ISchemaProvider>().Object);
        }

        [TestMethod]
        public void GivenAnUnrecognizedStatementType_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => builder.Build(null));
        }

        [TestMethod]
        public void GivenAnInsertTableStatement_ItBuildsAnInsertTableCommand()
        {
            var command = builder.Build(new InsertTableStatement(null));

            Assert.IsInstanceOfType(command, typeof(InsertTableCommand));
        }

        [TestMethod]
        public void GivenADeleteTableStatement_ItBuildsADeleteTableCommand()
        {
            var command = builder.Build(new DeleteTableStatement("", null));

            Assert.IsInstanceOfType(command, typeof(DeleteTableCommand));
        }

        [TestMethod]
        public void GivenAnInsertRecordStatement_ItBuildsAnInsertRecordCommand()
        {
            var command = builder.Build(new InsertRecordStatement("", null, null));

            Assert.IsInstanceOfType(command, typeof(InsertRecordCommand));
        }
    }
}
