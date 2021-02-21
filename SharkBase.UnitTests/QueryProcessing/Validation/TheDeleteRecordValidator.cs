using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.UnitTests.QueryProcessing.Validation
{
    [TestClass]
    public class TheDeleteRecordValidator
    {
        private DeleteRecordValidator validator;
        private Mock<ISchemaProvider> mockSchemas;
        private TableSchema schema;

        [TestInitialize]
        public void Initialize()
        {
            this.schema = new TableSchema("test", new List<Column> { new Column(ColumnType.Int64, "ID") });
            this.mockSchemas = new Mock<ISchemaProvider>();
            this.mockSchemas.Setup(schemas => schemas.GetSchema(schema.Name)).Returns(schema);
            this.validator = new DeleteRecordValidator(mockSchemas.Object);
        }

        [TestMethod]
        public void ThrowsAnExceptionIfAColumnDoesNotExist()
        {
            Assert.ThrowsException<ArgumentException>(() => validator.Validate(schema.Name, new List<string> { "name" }));
        }

        [TestMethod]
        public void ThrowsAnExceptionIfTheTableDoesNotExist()
        {
            Assert.ThrowsException<ArgumentException>(() => validator.Validate("", new List<string> { "ID", "21" }));
        }

        [TestMethod]
        public void ThrowsAnExceptionIfTheColumnTypeIsNotEqualToTheValuesType()
        {
            Assert.ThrowsException<ArgumentException>(() => validator.Validate(schema.Name, new List<string> { "ID", "hello world" }));
        }

        [TestMethod]
        public void ThrowsAnExceptionIfTokensLenghtIsNotEqualToTwo()
        {
            Assert.ThrowsException<ArgumentException>(() => validator.Validate(schema.Name, new List<string>() { "ID" }));
        }

        [TestMethod]
        public void GivenValidInputs_ItDoesNotThrowAnException()
        {
            validator.Validate(schema.Name, new List<string> { "ID", "21" });
        }
    }
}
