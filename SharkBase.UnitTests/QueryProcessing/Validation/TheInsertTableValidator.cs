using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;

namespace SharkBase.UnitTests.QueryProcessing.Validation
{
    [TestClass]
    public class TheInsertTableValidator
    {
        private InsertTableValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            validator = new InsertTableValidator();
        }

        [TestMethod]
        public void GivenAStatementWithNonexistingTypes_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => validator.Validate("FOOD", new List<string> { "STRING", "NAME" }));
        }
    }
}
