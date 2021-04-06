using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;

namespace SharkBase.UnitTests.QueryProcessing.Validation
{
    [TestClass]
    public class TheUpdateRecordValidator
    {
        private UpdateRecordValidator validator;
        private Mock<SchemaRepository> mockSchemas;
        private TableSchema schema;

        [TestInitialize]
        public void Initialize()
        {
            this.schema = new TableSchema("test", new List<Column> { new Column(DataTypes.Int64, "ID"), new Column(DataTypes.Int64, "COST") });
            this.mockSchemas = new Mock<SchemaRepository>();
            this.mockSchemas.Setup(schemas => schemas.GetSchema(schema.Name)).Returns(schema);
            this.validator = new UpdateRecordValidator(mockSchemas.Object);
        }

        [TestMethod]
        public void ThrowsAnExceptionIfAColumnDoesNotExist()
        {
            var statement = new UpdateRecordStatement("test", "no column", "");

            Assert.ThrowsException<ArgumentException>(() => validator.Validate(statement));
        }

        [TestMethod]
        public void ThrowsAnExceptionIfTheTableDoesNotExist()
        {
            var statement = new UpdateRecordStatement("na", "", "");

            Assert.ThrowsException<ArgumentException>(() => validator.Validate(statement));
        }

        [TestMethod]
        public void GivenAValidStatementWithNoWhereClause_ItDoesNotThrowAnException()
        {
            var statement = new UpdateRecordStatement("test", "ID", "9");

            validator.Validate(statement);
        }

        [TestMethod]
        public void ThrowsAnExceptionIfTheColumnTypeIsNotEqualToTheValuesType()
        {
            var statement = new UpdateRecordStatement("test", "ID", "hello");

            Assert.ThrowsException<ArgumentException>(() => validator.Validate(statement));
        }

        [TestMethod]
        public void ThrowsAnExceptionIfTheWhereColumnDoesNotExist()
        {
            var statement = new UpdateRecordStatement("test", "ID", "1", "no column", "value");

            Assert.ThrowsException<ArgumentException>(() => validator.Validate(statement));
        }

        [TestMethod]
        public void ThrowsAnExceptionIfTheWhereColumnTypeIsNotEqualToTheWhereValuesType()
        {
            var statement = new UpdateRecordStatement("test", "ID", "9", "COST", "hi");

            Assert.ThrowsException<ArgumentException>(() => validator.Validate(statement));
        }

        [TestMethod]
        public void GivenAValidStatementWithAWhereClause_ItDoesNotThrowAnException()
        {
            var statement = new UpdateRecordStatement("test", "ID", "9", "COST", "500");

            validator.Validate(statement);
        }
    }
}
