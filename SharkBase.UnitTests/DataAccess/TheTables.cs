using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharkBase.DataAccess;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;

namespace SharkBase.UnitTests.DataAccess
{
    [TestClass]
    public class TheTables
    {
        private Mock<ISystemStore> storeMock;
        private Mock<IGenerateId> mockIdGenerator;
        private Tables tables;
        private List<Column> columns;
        private TableSchema expectedSchema;

        [TestInitialize]
        public void Initialize()
        {
            storeMock = new Mock<ISystemStore>();
            mockIdGenerator = new Mock<IGenerateId>();
            tables = new Tables(storeMock.Object, new List<string> { "existing_table" }, new List<TableSchema>(), 
                new List<SharkBase.DataAccess.Index>(), mockIdGenerator.Object);
            columns = new List<Column> { new Column(ColumnType.Int64, "cost") };
            expectedSchema = new TableSchema(
               "test",
               new List<Column>
               {
                    new Column(ColumnType.String, "ID"),
                    new Column(ColumnType.String, "DELETED"),
                    new Column(ColumnType.Int64, "cost")
               }
           );
        }

        [TestMethod]
        public void WhenCreatingATable_ItInsertsTheTable()
        {
            tables.Create("test", columns);
            
            storeMock.Verify(store => store.InsertTable("test"), Times.Once);
        }

        [TestMethod]
        public void WhenCreatingATable_ItAddsItSchemaWithDefaultColumns()
        {
            tables.Create("test", columns);

            Assert.IsTrue(tables.GetSchema("test").Equals(expectedSchema));
        }

        [TestMethod]
        public void WhenATableIsCreated_ItShouldExist()
        {
            tables.Create("test", columns);

            Assert.IsTrue(tables.Exists("test"));
        }

        [TestMethod]
        public void WhenCreatingATableThatAlreadyExists_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => tables.Create("existing_table", new List<Column>()));
        }

        [TestMethod]
        public void WhenDeletingATable_ItDeletesTheTable()
        {
            tables.Delete("existing_table");

            storeMock.Verify(store => store.DeleteTable("existing_table"), Times.Once);
        }

        [TestMethod]
        public void AfterATableIsDeleted_ItShouldNotExist()
        {
            tables.Delete("existing_table");

            Assert.IsFalse(tables.Exists("existing_table"));
        }

        [TestMethod]
        public void WhenDeletingATableThatDoesNotExist_ItThrowsAnException()
        {
            Assert.ThrowsException<ArgumentException>(() => tables.Delete("aaa"));
        }

        [TestMethod]
        public void WhenGettingTheExistenceOfATable_IfTheTableExists_ItReturnsTrue()
        {
            Assert.IsTrue(tables.Exists("existing_table"));
        }

        [TestMethod]
        public void WhenGettingTheExistenceOfATable_IfTheTableDoesNotExist_ItReturnsFalse()
        {
            Assert.IsFalse(tables.Exists("nonexisting_table"));
        }
    }
}
