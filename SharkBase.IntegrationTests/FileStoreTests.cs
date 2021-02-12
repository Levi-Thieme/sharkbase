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

        private string TablePath(string name) => Path.Join(workingDirectory, name + ".table");
    }
}
