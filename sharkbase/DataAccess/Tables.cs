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

        public Tables(ISystemStore storage, IEnumerable<string> tables)
        {
            this.storage = storage;
            this.tables = tables.ToList();
        }

        public void Create(string name, IEnumerable<Column> columns)
        {
            if (exists(name))
                throw new ArgumentException($"The table, {name}, already exists.");
            storage.InsertTable(name);
            tables.Add(name);
        }

        public void Delete(string name)
        {
            if (!exists(name))
                throw new ArgumentException($"The table, {name}, does not exist.");
            storage.DeleteTable(name);
            tables.Remove(name);
        }

        public ITable GetByName(string name)
        {
            if (!exists(name))
                throw new ArgumentException($"The table, {name}, does not exist.");
            return new Table(this.storage, name);
        }

        public bool Exists(string name) => exists(name);

        private bool exists(string name) => tables.Contains(name);

        public void AddSchema(TableSchema table)
        {
            throw new NotImplementedException();
        }

        public void RemoveSchema(TableSchema table)
        {
            throw new NotImplementedException();
        }

        public TableSchema GetSchema(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
