using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.DataAccess.Index;
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
        private Mock<PhysicalStorage> mockStore;
        private Mock<IndexRepository> mockIndices;
        private TableSchema schema;
        private PrimaryIndex index;
        private Guid guid = Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81");
        private const string tableName = "test";

        [TestInitialize]
        public void Initialize()
        {
            mockIdGenerator = new Mock<IGenerateId>();
            mockStore = new Mock<PhysicalStorage>();
            mockIndices = new Mock<IndexRepository>();
            schema = new TableSchema(tableName, new List<Column>());
            index = new PrimaryIndex(tableName, new Dictionary<string, long>());
            table = new Table(mockStore.Object, schema, mockIndices.Object, mockIdGenerator.Object);
            mockIdGenerator.Setup(g => g.GetUniqueId()).Returns(guid);
            mockIndices.Setup(indices => indices.Get(tableName)).Returns(index);
        }

        [TestMethod]
        public void WhenGettingAUniqueId_ItReturnsTheGeneratedId()
        {
            Assert.AreEqual(guid, table.GetUniqueId());
        }

        [TestClass]
        public class WhenInsertingARecord : TheTable
        {
            [TestMethod]
            public void WhenInsertingARecord_ItGetsANewGuid()
            {
                table.InsertRecord(new Record(new List<Value>()));

                mockIdGenerator.Verify(g => g.GetUniqueId(), Times.Once);
            }

            [TestMethod]
            public void ItGetsThePrimaryIndex()
            {
                table.InsertRecord(new Record(new List<Value>()));

                mockIndices.Verify(indices => indices.Get(tableName), Times.Once);
            }

            [TestMethod]
            public void WhenInsertingARecord_ItUpdatesThePrimaryIndex()
            {
                mockStore.Setup(store => store.Append(It.IsAny<string>(), It.IsAny<MemoryStream>())).Returns(35);

                table.InsertRecord(new Record(new List<Value>()));

                mockIndices.Verify(indices => indices.Upsert(index), Times.Once);
            }
        }
    }
}
