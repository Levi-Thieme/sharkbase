using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.SystemStorage;
using System;
using System.Collections;
using System.Collections.Generic;
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

        [TestInitialize]
        public void Initialize()
        {
            schema = new TableSchema("test", new List<Column> { new Column(ColumnType.Int64, "ID"), new Column(ColumnType.Char128, "NAME"), new Column(ColumnType.Int64, "COST") });
            var store = new FileStore("C:/Users/signo/Downloads/test_db");
            store.DeleteTable("test");
            store.InsertTable("test");
            this.table = new Table(store, schema);
            var parser = new ValueParser();
            var pizza = parser.ParseString("Pizza");
            record = new Record(21L, pizza, 9001L);
        }

        [TestMethod]
        public void WritesRecords()
        {
            int count = 2;
            
            for (int i = 0; i < count; i++)
                table.InsertRecord(record);

            Assert.AreEqual(count, table.RecordCount());
        }

        [TestMethod]
        public void WritesAndReadsRecords()
        {
            table.InsertRecord(record);

            var readRecords = table.ReadFirstNRecord(1);

            CollectionAssert.AreEqual(record.Values.ToList(), readRecords.ElementAt(0).Values.ToList());
        }

        [TestMethod]
        public void WritesReadAndWrites()
        {
            int count = 10;

            for (int i = 0; i < count; i++)
                table.InsertRecord(record);

            _ = table.ReadFirstNRecord(count);

            for (int i = 0; i < count; i++)
                table.InsertRecord(record);

            var allRecords = table.ReadAllRecords();

            Assert.AreEqual(20, allRecords.Count());
        }
    }
}
