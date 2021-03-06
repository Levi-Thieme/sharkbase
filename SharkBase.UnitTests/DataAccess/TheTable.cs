using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.DataAccess.Index;
using SharkBase.DataAccess.Index.Models;
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

        

        [TestClass]
        public class WhenGettingAUniqueId : TheTable
        {
            [TestInitialize]
            public void Initialize()
            {
                mockIdGenerator = new Mock<IGenerateId>();
                mockStore = new Mock<PhysicalStorage>();
                mockIndices = new Mock<IndexRepository>();
                schema = new TableSchema(tableName, new List<Column>());
                table = new Table(mockStore.Object, schema, mockIndices.Object, mockIdGenerator.Object);
            }

            [TestMethod]
            public void ItGetsAUniqueIdFromTheGenerator()
            {
                table.GetUniqueId();

                mockIdGenerator.Verify(generator => generator.GetUniqueId(), Times.Once);
            }

            [TestMethod]
            public void ItReturnsTheGeneratedId()
            {
                mockIdGenerator.Setup(g => g.GetUniqueId()).Returns(guid);

                Assert.AreEqual(guid, table.GetUniqueId());
            }
        }

        [TestClass]
        public class WhenInsertingARecord : TheTable
        {
            SecondaryIndex<bool> deletedIndex;

            [TestInitialize]
            public void Initialize()
            {
                mockIdGenerator = new Mock<IGenerateId>();
                mockStore = new Mock<PhysicalStorage>();
                mockIndices = new Mock<IndexRepository>();
                schema = new TableSchema(tableName, new List<Column>());
                index = new PrimaryIndex(tableName, new Dictionary<string, long>());
                deletedIndex = new SecondaryIndex<bool>(tableName, IndexNames.IS_DELETED, new Dictionary<string, bool>());
                table = new Table(mockStore.Object, schema, mockIndices.Object, mockIdGenerator.Object);
                mockIdGenerator.Setup(g => g.GetUniqueId()).Returns(guid);
                mockIndices.Setup(indices => indices.Get(tableName)).Returns(index);
                mockIndices.Setup(indices => indices.GetIsDeletedIndex(tableName)).Returns(deletedIndex);
                mockStore.Setup(store => store.GetTableStream(It.IsAny<string>())).Returns(new MemoryStream());
            }

            [TestMethod]
            public void ItGetsANewGuid()
            {
                table.InsertRecord(new Record(new List<Value>()));

                mockIdGenerator.Verify(g => g.GetUniqueId(), Times.Once);
            }

            [TestMethod]
            public void ItGetsThePrimaryIndexOnce()
            {
                table.InsertRecord(new Record(new List<Value>()));

                mockIndices.Verify(indices => indices.Get(tableName), Times.Once);
            }

            [TestMethod]
            public void ItUpdatesThePrimaryIndex()
            {
                table.InsertRecord(new Record(new List<Value>()));

                mockIndices.Verify(indices => indices.Upsert(index), Times.Once);
            }

            [TestMethod]
            public void ItGetsTheIsDeletedIndexOnce()
            {
                table.InsertRecord(new Record(new List<Value>()));

                mockIndices.Verify(indices => indices.GetIsDeletedIndex(tableName), Times.Once);
            }

            [TestMethod]
            public void ItUpdatesTheDeletedIndex()
            {
                table.InsertRecord(new Record(new List<Value>()));

                mockIndices.Verify(indices => indices.Upsert<bool>(deletedIndex), Times.Once);
            }
        }

        [TestClass]
        public class WhenDeletingARecord : TheTable
        {
            private SecondaryIndex<bool> isDeletedIndex;

            [TestInitialize]
            public void Initialize()
            {
                mockIdGenerator = new Mock<IGenerateId>();
                mockStore = new Mock<PhysicalStorage>();
                mockIndices = new Mock<IndexRepository>();
                schema = new TableSchema(tableName, new List<Column>());
                index = new PrimaryIndex(tableName, new Dictionary<string, long> { { guid.ToString(), 0L } });
                isDeletedIndex = new SecondaryIndex<bool>(tableName, IndexNames.IS_DELETED, new Dictionary<string, bool> { { guid.ToString(), false } });
                table = new Table(mockStore.Object, schema, mockIndices.Object, mockIdGenerator.Object);
                mockIdGenerator.Setup(g => g.GetUniqueId()).Returns(guid);
                mockIndices.Setup(indices => indices.Get(tableName)).Returns(index);
                mockIndices.Setup(indices => indices.GetIsDeletedIndex(tableName)).Returns(isDeletedIndex);
                mockStore.Setup(store => store.GetTableStream(It.IsAny<string>())).Returns(new MemoryStream());
            }

            [TestMethod]
            public void ItGetsThePrimaryIndex()
            {
                var record = new Record(new List<Value> { new Value(guid), new Value(false), new Value(100L) });

                table.DeleteRecord(record);

                mockIndices.Verify(indices => indices.Get(schema.Name), Times.Once);
            }

            [TestMethod]
            public void ItGetsTheIsDeletedIndex()
            {
                var record = new Record(new List<Value> { new Value(guid), new Value(false), new Value(100L) });

                table.DeleteRecord(record);

                mockIndices.Verify(indices => indices.GetIsDeletedIndex(schema.Name), Times.Once);
            }

            [TestMethod]
            public void ItSetsTheRecordAsDeletedInTheIsDeletedIndex()
            {
                var record = new Record(new List<Value> { new Value(guid), new Value(false), new Value(100L) });

                table.DeleteRecord(record);

                Assert.IsTrue(isDeletedIndex.GetValue(record.GetId()));
            }

            [TestMethod]
            public void ItUpsertsTheIsDeletedIndex()
            {
                var record = new Record(new List<Value> { new Value(guid), new Value(false), new Value(100L) });

                table.DeleteRecord(record);

                mockIndices.Verify(indices => indices.Upsert<bool>(isDeletedIndex), Times.Once);
            }
        }
    }
}
