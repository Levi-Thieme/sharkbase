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
        private readonly string workingDirectory = Path.Join(Path.GetTempPath(), "test_db");

        [TestInitialize]
        public void Initialize()
        {
            if (!Directory.Exists(workingDirectory))
                Directory.CreateDirectory(workingDirectory);
            this.store = new FileStore(workingDirectory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(workingDirectory, true);
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
        public void ReadsRecords()
        {
            File.Create(TablePath("test")).Dispose();

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

        private string TablePath(string name) => Path.Join(workingDirectory, name + ".table");
    }
}
