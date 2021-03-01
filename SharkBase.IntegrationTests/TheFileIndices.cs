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
        private const string table = "test";
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
            primaryIndex = new PrimaryIndex(table, new Dictionary<string, long> { { "12345", 0L } });
            secondaryIndex = new SecondaryIndex<bool>(table, "isDeleted", new Dictionary<string, bool> { { "12345", true } });
            Directory.CreateDirectory(Path.Combine(databaseDirectory, table));
            File.Create(Path.Combine(databaseDirectory, table, $"{table}.table")).Dispose();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(databaseDirectory, true);
        }

        [TestMethod]
        public void GivenATableName_WhenRemovingAllIndices_ItDeletesThem()
        {
            indices.Upsert(primaryIndex);
            indices.Upsert<bool>(secondaryIndex);

            indices.RemoveAll(table);

            Assert.IsFalse(File.Exists(store.IndexFilePath(table, primaryIndex.Name)));
            Assert.IsFalse(File.Exists(store.IndexFilePath(table, secondaryIndex.Name)));
        }

        [TestMethod]
        public void GivenATableName_WhenAddingAPrimaryIndex_ItUpsertsThePrimaryIndex()
        {
            var expectedIndex = new PrimaryIndex(table, new Dictionary<string, long>());
            
            indices.AddPrimaryIndex(table);

            var actualIndex = indices.Get(table);
            Assert.AreEqual(expectedIndex, actualIndex);
        }

        [TestClass]
        public class GivenAPrimaryIndex : TheFileIndices
        {
            [TestMethod]
            public void AndItDoesNotExist_WhenUpserting_ItIsCreated()
            {
                indices.Upsert(primaryIndex);

                Assert.IsTrue(File.Exists(store.IndexFilePath(table, primaryIndex.Name)));
            }

            [TestMethod]
            public void WhenGettingAnIndex_ItReturnsTheIndex()
            {
                indices.Upsert(primaryIndex);

                var retrievedIndex = indices.Get(table);

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

                Assert.IsTrue(File.Exists(store.IndexFilePath(table, secondaryIndex.Name)));
            }

            [TestMethod]
            public void WhenGettingAnIndex_ItReturnsTheIndex()
            {
                indices.Upsert(secondaryIndex);

                var retrievedIndex = indices.Get<bool>(table, secondaryIndex.Name);

                Assert.AreEqual(secondaryIndex, retrievedIndex);
            }
        }

        [TestClass]
        public class WhenUpsertingAnIndex : TheFileIndices
        {
            [TestMethod]
            public void AndAnIndexHasBeenRemoved_ItOverwritesTheIndex()
            {
                primaryIndex.Add("second", 21L);
                indices.Upsert(primaryIndex);

                primaryIndex.Indices.Remove("second");
                indices.Upsert(primaryIndex);

                var retrievedIndex = indices.Get(table);

                Assert.AreEqual(primaryIndex, retrievedIndex);
            }
        }
    }
}
