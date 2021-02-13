using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.SystemStorage;
using System;
using System.Collections;
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
        private TableSchema schema;
        private Record record;
        private readonly string databaseDirectory = Path.Join(Path.GetTempPath(), "test_db");

        [TestInitialize]
        public void Initialize()
        {
            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory);
            schema = new TableSchema("test", new List<Column> { new Column(ColumnType.Int64, "ID"), new Column(ColumnType.String, "NAME"), new Column(ColumnType.Int64, "COST") });
            var store = new FileStore(databaseDirectory);
            store.DeleteTable("test");
            store.InsertTable("test");
            this.table = new Table(store, schema);
            var parser = new ValueParser();
            var pizza = parser.ParseString("Pizza");
            record = new Record(21L, pizza, 9001L);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(databaseDirectory, true);
        }

        [TestMethod]
        public void WritesRecords()
        {
            table.InsertRecord(record);

            long length = new FileInfo(tablePath("test")).Length;
            Assert.AreEqual(22, length);
        }

        [TestMethod]
        public void ReadsARecord()
        {
            table.InsertRecord(record);

            var readRecord = table.ReadRecord();

            Assert.AreEqual(record, readRecord);
        }

        [TestMethod]
        public void ReadsAllRecords()
        {
            var expectedRecords = new List<Record> 
            { 
                new Record(1L, "Tacos", 5L), 
                new Record(2L, "Pizza", 8L), 
                new Record(3L, "Steak", 16L)
            };
            foreach (var rec in expectedRecords)
            {
                table.InsertRecord(rec);
            }

            var actualRecords = table.ReadAllRecords();

            CollectionAssert.AreEqual(expectedRecords, actualRecords.ToList());
        }

        private string tablePath(string name) => Path.Combine(databaseDirectory, name + ".table");
    }
}
