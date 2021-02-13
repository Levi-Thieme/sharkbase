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

        public Tables(ISystemStore storage, IEnumerable<string> tables, IEnumerable<TableSchema> schemas)
        {
            this.storage = storage;
            this.tables = tables.ToList();
            this.schemas = schemas.ToList();
        }

        public void Create(string name, IEnumerable<Column> columns)
        {
            if (exists(name))
                throw new ArgumentException($"The table, {name}, already exists.");
            schemas.Add(new TableSchema(name, columns));
            storage.InsertTable(name);
            tables.Add(name);
        }

        public void Delete(string name)
        {
            if (!exists(name))
                throw new ArgumentException($"The table, {name}, does not exist.");
            schemas.RemoveAll(schema => schema.Name == name);
            storage.DeleteTable(name);
            tables.Remove(name);
        }

        public ITable GetByName(string name)
        {
            if (!exists(name))
                throw new ArgumentException($"The table, {name}, does not exist.");
            return new Table(this.storage, getSchema(name));
        }

        public IEnumerable<TableSchema> GetAllSchemas() => this.schemas;

        public bool Exists(string name) => exists(name);

        private bool exists(string name) => tables.Contains(name);

        public void AddSchema(TableSchema table) => schemas.Add(table);

        public void RemoveSchema(TableSchema table) => schemas.Remove(table);

        public TableSchema GetSchema(string tableName) => getSchema(tableName);

        private TableSchema getSchema(string tableName) => schemas.FirstOrDefault(schema => schema.Name == tableName);
    }
}
