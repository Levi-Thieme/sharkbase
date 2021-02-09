using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.DataAccess;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharkBase.IntegrationTests
{
    [TestClass]
    public class FileStoreTests
    {
        private FileStore store;
        private string workingDirectory;

        [TestInitialize]
        public void Initialize()
        {
            this.workingDirectory = "C:/Users/Levi/Downloads/test_db";
            this.store = new FileStore(workingDirectory);
        }

        [TestMethod]
        public void CreatesATable()
        {
            store.InsertTable("MYTABLE");

            Assert.IsTrue(File.Exists(TablePath("MYTABLE")));
        }

        [TestMethod]
        public void DeletesATable()
        {
            File.Create(TablePath("MYTABLE")).Dispose();

            store.DeleteTable("MYTABLE");

            Assert.IsFalse(File.Exists(TablePath("MYTABLE")));
        }

        [TestMethod]
        public void InsertsRecordsIntoATable()
        {
            File.Create(TablePath("test")).Dispose();
            var values = new List<object>
            {
                21L,
                "I like food."
            };

            for (int i = 0; i < 10000; i++)
            {
                store.InsertRecord("test", values);
            }

            var readValues = store.ReadRecord("test", new TableSchema("test", new List<Column> { new Column(ColumnType.Int64, ""), new Column(ColumnType.Char128, "") }));

            CollectionAssert.AreEqual(values, readValues.ToList());
        }

        [TestMethod]
        public void memoryStreamTests()
        {
            var values = new List<object>
            {
                21L,
                "I like food."
            };
            using (MemoryStream stream = new MemoryStream())
            {
                for(int i = 0; i < 10000; i++)
                {
                    stream.Write(BitConverter.GetBytes(21L));
                }
                store.WriteFromPosition("test", stream, 0L);
            }
        }

        private string TablePath(string name) => Path.Join(workingDirectory, name + ".table");
    }
}
