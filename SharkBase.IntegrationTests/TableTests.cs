using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.Models;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharkBase.IntegrationTests
{
    [TestClass]
    public class TableTests
    {
        private Table table;
        private TableSchema schema;
        private Record insertionRecord;
        private Record expectedRecord;
        private readonly string databaseDirectory = Path.Join(Path.GetTempPath(), "Integration_test_db");
        private Guid guid = Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81");
        private Mock<IGenerateId> mockIdGenerator;

        [TestInitialize]
        public void Initialize()
        {
            mockIdGenerator = new Mock<IGenerateId>();
            mockIdGenerator.Setup(g => g.GetUniqueId()).Returns(guid);
            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory);
            schema = new TableSchema("test", new List<Column> { new Column(ColumnType.Int64, "ID"), new Column(ColumnType.String, "NAME"), new Column(ColumnType.Int64, "COST") });
            var store = new FileStore(databaseDirectory);
            store.DeleteTable("test");
            store.InsertTable("test");
            this.table = new Table(store, schema, new DataAccess.Index("test", new Dictionary<string, long>()), mockIdGenerator.Object);
            insertionRecord = new Record(new List<Value> { new Value(21L), new Value("pizza"), new Value(9001L) });
            expectedRecord = new Record(new List<Value> { new Value(guid.ToString()), new Value(21L), new Value("pizza"), new Value(9001L) });
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(databaseDirectory, true);
        }

        [TestMethod]
        public void WritesRecords()
        {
            table.InsertRecord(insertionRecord);

            long length = new FileInfo(tablePath("test")).Length;
            Assert.AreEqual(59, length);
        }

        [TestMethod]
        public void ReadsARecord()
        {
            table.InsertRecord(insertionRecord);

            var readRecord = table.ReadRecord();

            Assert.AreEqual(expectedRecord, readRecord);
        }

        [TestMethod]
        public void ReadsAllRecords()
        {
            mockIdGenerator.SetupSequence(g => g.GetUniqueId())
                .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81"))
                .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f82"))
                .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f83"));
            var recordsToInsert = new List<Record> 
            { 
                new Record(new List<Value> { new Value(1L), new Value("Tacos"), new Value(5L) }), 
                new Record(new List<Value> { new Value(2L), new Value("pizza"), new Value(9L) }), 
                new Record(new List<Value> { new Value(3L), new Value("steak"), new Value(16L) })
            };
            foreach (var rec in recordsToInsert)
            {
                table.InsertRecord(rec);
            }
            var expectedRecords = new List<Record>
            {
                new Record(new List<Value> { new Value("6b7ad35b-8176-4139-9f60-fa5654412f81"), new Value(1L), new Value("Tacos"), new Value(5L) }),
                new Record(new List<Value> { new Value("6b7ad35b-8176-4139-9f60-fa5654412f82"), new Value(2L), new Value("pizza"), new Value(9L) }),
                new Record(new List<Value> { new Value("6b7ad35b-8176-4139-9f60-fa5654412f83"), new Value(3L), new Value("steak"), new Value(16L) })
            };

            var actualRecords = table.ReadAllRecords();

            CollectionAssert.AreEqual(expectedRecords, actualRecords.ToList());
        }

        private string tablePath(string name) => Path.Combine(databaseDirectory, name + ".table");
    }
}
