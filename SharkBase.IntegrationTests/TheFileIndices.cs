using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.DataAccess.Index;
using SharkBase.DataAccess.Index.Repositories;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.IntegrationTests
{
    [TestClass]
    public class TheFileIndices
    {
        private const string database = "Integration_test_db";
        private readonly string databaseDirectory = Path.Join(Path.GetTempPath(), database);
        FileStore store;
        FileIndices indices;
        PrimaryIndex primaryIndex;
        SecondaryIndex<bool> secondaryIndex;

        [TestInitialize]
        public void Initialize()
        {
            Directory.CreateDirectory(databaseDirectory);
            store = new FileStore(databaseDirectory);
            indices = new FileIndices(store);
            primaryIndex = new PrimaryIndex("test", new Dictionary<string, long> { { "12345", 0L } });
            secondaryIndex = new SecondaryIndex<bool>("test", "isDeleted", new Dictionary<string, bool> { { "12345", true } });
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(databaseDirectory, true);
        }

        [TestClass]
        public class GivenAPrimaryIndex : TheFileIndices
        {
            [TestMethod]
            public void AndItDoesNotExist_WhenUpserting_ItIsCreated()
            {
                indices.Upsert(primaryIndex);

                Assert.IsTrue(File.Exists(store.IndexFilePath(primaryIndex.Name)));
            }

            [TestMethod]
            public void WhenGettingAnIndex_ItReturnsTheIndex()
            {
                indices.Upsert(primaryIndex);

                var retrievedIndex = indices.Get(primaryIndex.Name);

                Assert.AreEqual(primaryIndex, retrievedIndex);
            }
        }

        [TestClass]
        public class GivenASecondaryIndex : TheFileIndices
        {
            [TestMethod]
            public void AndItDoesNotExist_WhenUpsertingItIsCreated()
            {
                indices.Upsert(secondaryIndex);

                Assert.IsTrue(File.Exists(store.IndexFilePath(secondaryIndex.Name)));
            }

            [TestMethod]
            public void WhenGettingAnIndex_ItReturnsTheIndex()
            {
                indices.Upsert(secondaryIndex);

                var retrievedIndex = indices.Get<bool>(secondaryIndex.Name);

                Assert.AreEqual(secondaryIndex, retrievedIndex);
            }
        }
    }
}
