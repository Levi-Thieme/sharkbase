using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.DataAccess
{
    public class Tables : ITables, ISchemaProvider
    {
        private ISystemStore storage;
        private List<string> tables;
        private List<TableSchema> schemas;
        private List<DataAccess.Index> indices;

        public Tables(ISystemStore storage, IEnumerable<string> tables, IEnumerable<TableSchema> schemas, IEnumerable<DataAccess.Index> indices)
        {
            this.storage = storage;
            this.tables = tables.ToList();
            this.schemas = schemas.ToList();
            this.indices = indices.ToList();
        }

        public void Create(string name, IEnumerable<Column> columns)
        {
            if (exists(name))
                throw new ArgumentException($"The table, {name}, already exists.");
            schemas.Add(new TableSchema(name, columns));
            indices.Add(new Index(name, new Dictionary<string, long>()));
            storage.InsertTable(name);
            tables.Add(name);
        }

        public void Delete(string name)
        {
            if (!exists(name))
                throw new ArgumentException($"The table, {name}, does not exist.");
            schemas.RemoveAll(schema => schema.Name == name);
            indices.RemoveAll(i => i.Table == name);
            storage.DeleteTable(name);
            tables.Remove(name);
        }

        public ITable GetByName(string name)
        {
            if (!exists(name))
                throw new ArgumentException($"The table, {name}, does not exist.");
            var index = this.indices.FirstOrDefault(i => i.Table == name);
            if (index == null)
            {
                index = new Index(name, new Dictionary<string, long>());
                this.indices.Add(index);
            }
            return new Table(this.storage, getSchema(name), index);
        }

        public IEnumerable<Index> GetIndices() => this.indices.ToList();

        public IEnumerable<TableSchema> GetAllSchemas() => this.schemas;

        public bool Exists(string name) => exists(name);

        private bool exists(string name) => tables.Contains(name);

        public void AddSchema(TableSchema table) => schemas.Add(table);

        public void RemoveSchema(TableSchema table) => schemas.Remove(table);

        public TableSchema GetSchema(string tableName) => getSchema(tableName);

        private TableSchema getSchema(string tableName) => schemas.FirstOrDefault(schema => schema.Name == tableName);
    }
}
