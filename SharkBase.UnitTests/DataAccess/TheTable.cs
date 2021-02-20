using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.Models;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.UnitTests.DataAccess
{
    [TestClass]
    public class TheTable
    {
        private Table table;
        private Mock<IGenerateId> mockIdGenerator;
        private Mock<ISystemStore> mockStore;
        private TableSchema schema;
        private SharkBase.DataAccess.Index index;
        private Guid guid = Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81");

        [TestInitialize]
        public void Initialize()
        {
            mockIdGenerator = new Mock<IGenerateId>();
            mockStore = new Mock<ISystemStore>();
            schema = new TableSchema("test", new List<Column>());
            index = new SharkBase.DataAccess.Index("test", new Dictionary<string, long>());
            table = new Table(mockStore.Object, schema, index, mockIdGenerator.Object);
            mockIdGenerator.Setup(g => g.GetUniqueId()).Returns(guid);
        }

        [TestMethod]
        public void WhenGettingAUniqueId_ItReturnsTheGeneratedId()
        {
            Assert.AreEqual(guid, table.GetUniqueId());
        }

        [TestMethod]
        public void WhenInsertingARecord_ItGetsANewGuid()
        {
            table.InsertRecord(new Record(new List<Value>()));

            mockIdGenerator.Verify(g => g.GetUniqueId(), Times.Once);
        }

        [TestMethod]
        public void WhenInsertingARecord_ItAddsThePositionToTheIndex()
        {
            mockStore.Setup(store => store.Append(It.IsAny<string>(), It.IsAny<MemoryStream>())).Returns(35);

            table.InsertRecord(new Record(new List<Value>()));

            Assert.AreEqual(35, index.GetValue(guid.ToString()));
        }
    }
}
