using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.DataAccess;
using SharkBase.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.UnitTests.DataAccess
{
    [TestClass]
    public class TheRecord
    {
        private Record record;

        [TestInitialize]
        public void Initialize()
        {
            record = buildRecord();
        }

        [TestMethod]
        public void GivenARecordWithSecondValueEqualToTrue_ItIsDeleted()
        {
            var deletedRecord = buildDeletedRecord();

            Assert.IsTrue(deletedRecord.IsDeleted());
        }

        [TestMethod]
        public void GivenARecordWithSecondValueEqualToFalse_ItIsNotDeleted()
        {
            var deletedRecord = buildRecord();

            Assert.IsFalse(deletedRecord.IsDeleted());
        }

        [TestMethod]
        public void WhenDeletingARecord_ItIsDeleted()
        {
            record.Delete();

            Assert.IsTrue(record.IsDeleted());
        }

        [TestMethod]
        public void WhenDeletingARecordWithoutIdAndDeletedFields_ItThrowsAnException()
        {
            var invalidRecord = new Record(new List<Value> { new Value(false) });

            Assert.ThrowsException<Exception>(() => invalidRecord.Delete());
        }

        private Record buildRecord()
        {
            var values = new List<Value>
            {
                new Value(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81").ToString()),
                new Value(false),
                new Value("Tacos"),
                new Value("6")
            };
            return new Record(values);
        }

        private Record buildDeletedRecord()
        {
            var values = new List<Value>
            {
                new Value(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81").ToString()),
                new Value(true),
                new Value("Tacos"),
                new Value("6")
            };
            return new Record(values);
        }
    }
}
