using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.DataAccess.Index;
using SharkBase.DataAccess.Index.Repositories;
using SharkBase.DataAccess.Schema.Repositories;
using SharkBase.Models;
using SharkBase.Models.Values;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharkBase.IntegrationTests
{
    [TestClass]
    public class TableTests
    {
        private Table table;
        private Record insertionRecord;
        private Record expectedRecord;
        private readonly string databaseDirectory = Path.Join(Path.GetTempPath(), "Integration_test_db");
        private Guid guid = Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81");
        private const string tableName = "test";
        private Mock<IGenerateId> mockIdGenerator;
        private IndexRepository indices; 

        [TestInitialize]
        public void Initialize()
        {
            mockIdGenerator = new Mock<IGenerateId>();
            mockIdGenerator.Setup(g => g.GetUniqueId()).Returns(guid);
            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory);
            var store = new FileStore(databaseDirectory);
            this.indices = new FileIndices(store);
            var tables = new Tables(store, mockIdGenerator.Object, new FileSchemas(store), new FileIndices(store), new List<string>());
            var columns = new List<Column>
            {
                new Column(DataTypes.String, "NAME"),
                new Column(DataTypes.Int64, "COST")
            };
            tables.Create(tableName, columns);
            this.table = tables.GetByName(tableName) as Table;
            insertionRecord = new Record(new List<Value> { new StringValue("pizza"), new LongValue(9001L) });
            expectedRecord = new Record(new List<Value> { new StringValue(guid.ToString()), new StringValue("pizza"), new LongValue(9001L) });
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
        public void ReadsAllRecords()
        {
            mockIdGenerator.SetupSequence(g => g.GetUniqueId())
                .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81"))
                .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f82"))
                .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f83"));
            var recordsToInsert = new List<Record> 
            { 
                new Record(new List<Value> { new StringValue("Tacos"), new LongValue(5L) }), 
                new Record(new List<Value> { new StringValue("pizza"), new LongValue(9L) }), 
                new Record(new List<Value> { new StringValue("steak"), new LongValue(16L) })
            };
            foreach (var rec in recordsToInsert)
            {
                table.InsertRecord(rec);
            }
            var expectedRecords = new List<Record>
            {
                new Record(new List<Value> { new StringValue("6b7ad35b-8176-4139-9f60-fa5654412f81"), new StringValue("Tacos"), new LongValue(5L) }),
                new Record(new List<Value> { new StringValue("6b7ad35b-8176-4139-9f60-fa5654412f82"), new StringValue("pizza"), new LongValue(9L) }),
                new Record(new List<Value> { new StringValue("6b7ad35b-8176-4139-9f60-fa5654412f83"), new StringValue("steak"), new LongValue(16L) })
            };

            using (var records = table.ReadAll())
            {
                int index = 0;
                while (records.Read())
                {
                    var currentRecord = records.Current;
                    Assert.AreEqual(expectedRecords[index], currentRecord);
                    index += 1;
                }
            }
        }

        [TestMethod]
        public void GivenDeletedRecordsExist_WhenReadingAllRecords_ItOnlyReturnsNonDeletedOnes()
        {
            mockIdGenerator.SetupSequence(g => g.GetUniqueId())
                            .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81"))
                            .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f82"))
                            .Returns(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f83"));
            var recordsToInsert = new List<Record>
            {
                new Record(new List<Value> { new StringValue("Tacos"), new LongValue(5L) }),
                new Record(new List<Value> { new StringValue("pizza"), new LongValue(9L) }),
                new Record(new List<Value> { new StringValue("steak"), new LongValue(16L) })
            };
            foreach (var rec in recordsToInsert)
            {
                table.InsertRecord(rec);
            }
            table.DeleteRecord(table.ReadAllRecords().ElementAt(1));

            var expectedRecords = new List<Record>
            {
                new Record(new List<Value> { new StringValue("6b7ad35b-8176-4139-9f60-fa5654412f81"), new StringValue("Tacos"), new LongValue(5L) }),
                new Record(new List<Value> { new StringValue("6b7ad35b-8176-4139-9f60-fa5654412f83"), new StringValue("steak"), new LongValue(16L) })
            };

            var actualRecords = new List<Record>();
            using (var recordStream = table.ReadAll())
            {
                while (recordStream.Read())
                    actualRecords.Add(recordStream.Current);
            }

            CollectionAssert.AreEqual(expectedRecords, actualRecords);
        }

        private string tablePath(string name) => Path.Combine(databaseDirectory, name, $"{name}.table");
    }
}
