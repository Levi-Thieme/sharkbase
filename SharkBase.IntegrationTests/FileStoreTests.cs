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
        private const string databaseName = "integration_test_db";
        private readonly string workingDirectory = Path.Join(Path.GetTempPath(), databaseName);
        private const string tableName = "test";

        [TestInitialize]
        public void Initialize()
        {
            if (!Directory.Exists(workingDirectory))
                Directory.CreateDirectory(workingDirectory);
            this.store = new FileStore(workingDirectory);
            Directory.CreateDirectory(Path.Combine(workingDirectory, tableName));
            File.Create(TablePath(tableName)).Dispose();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(workingDirectory, true);
        }

        [TestMethod]
        public void CreatesATable()
        {
            store.InsertTable(tableName);

            Assert.IsTrue(File.Exists(TablePath(tableName)));
        }

        [TestMethod]
        public void DeletesATable()
        {
            store.DeleteTable(tableName);

            Assert.IsFalse(File.Exists(TablePath(tableName)));
        }

        [TestMethod]
        public void WhenGettingTheDatabaseMetadataStream_AndTheFileDoesNotExist_ItCreatesIt()
        {
            store.GetDatabaseMetadataStream().Dispose();
            
            Assert.IsTrue(File.Exists(Path.Join(workingDirectory, $"{databaseName}.metadata")));
        }

        [TestMethod]
        public void ReadsRecords()
        {
            var fstream = new FileStream(TablePath("test"), FileMode.Open);
            using (BinaryWriter reader = new BinaryWriter(fstream, System.Text.Encoding.UTF8))
            {
                reader.Write("Hello World!");
            }
            fstream = new FileStream(TablePath("test"), FileMode.Open);
            var offsets = new List<long>();
            using (BinaryReader reader = new BinaryReader(fstream))
            {
                reader.ReadString();
                offsets.Add(reader.BaseStream.Position);
            }
        }

        [TestMethod]
        public void TestStringOverWriteWithBinaryWriter()
        {
            var writeOffsets = new List<long>();
            var fstream = new FileStream(TablePath("test"), FileMode.Open);
            using (BinaryWriter reader = new BinaryWriter(fstream, System.Text.Encoding.UTF8))
            {
                writeOffsets.Add(reader.BaseStream.Position);
                reader.Write("Bye");
            }

            fstream = new FileStream(TablePath("test"), FileMode.Open);
            using (BinaryWriter reader = new BinaryWriter(fstream, System.Text.Encoding.UTF8))
            {
                reader.Write("Hello beautiful World!");
                reader.Write(21L);
                reader.Write("ok");
            }

            fstream = new FileStream(TablePath("test"), FileMode.Open);
            var readOffsets = new List<long>();
            using (BinaryReader reader = new BinaryReader(fstream))
            {
                readOffsets.Add(reader.BaseStream.Position);
                string text = reader.ReadString();
                Assert.AreEqual("Hello beautiful World!", text);
                
                long remaining = reader.ReadInt64();
                Assert.AreEqual(21L, remaining);

                string maybeText = reader.ReadString();
                Assert.AreEqual("ok", maybeText);
            }

            CollectionAssert.AreEqual(writeOffsets, readOffsets);
        }

        private string TablePath(string name) => Path.Join(workingDirectory, name, $"{name}.table");
    }
}
