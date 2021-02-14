using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;

namespace SharkBase.UnitTests.QueryProcessing.Validation
{
    [TestClass]
    public class TheInsertRecordValidator
    {
        private Mock<ISchemaProvider> mockSchemas;
        private InsertRecordValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            mockSchemas = new Mock<ISchemaProvider>();
            mockSchemas.Setup(schemas => schemas.GetSchema("Foods"))
                .Returns(new TableSchema("Foods", new List<Column> { new Column(ColumnType.Int64, "ID") }));
            validator = new InsertRecordValidator(mockSchemas.Object);
        }

        [TestMethod]
        public void GivenTheNumberOfTokensIsLessThanTheNumberOfColumns_WhenValidating_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => validator.Validate("Foods", new List<string>()));
        }

        [TestMethod]
        public void GivenTheNumberOfTokensIsMoreThanTheNumberOfColumns_WhenValidating_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => validator.Validate("Foods", new List<string>() { "", "", "" }));
        }

        [TestMethod]
        public void GivenTheDataTypeOfAColumnValueIsNotParsableToTheColumnType_WhenValidating_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => validator.Validate("Foods", new List<string>() { "Tacos are good" }));
        }
    }
}
