using SharkBase.DataAccess.Index;
using SharkBase.DataAccess.Index.Models;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.DataAccess
{
    public class Tables : ITables 
    {
        private PhysicalStorage storage;
        private IGenerateId idGenerator;
        private SchemaRepository schemas;
        private IndexRepository indices;
        private List<string> tables;
        public IEnumerable<string> TableNames() => tables;
        
        public Tables(PhysicalStorage storage, IGenerateId idGenerator, SchemaRepository schemas, IndexRepository indices, IEnumerable<string> tables)
        {
            this.storage = storage;
            this.idGenerator = idGenerator;
            this.schemas = schemas;
            this.indices = indices;
            this.tables = tables.ToList();
        }

        public void Create(string tableName, IEnumerable<Column> columns)
        {
            if (exists(tableName))
                throw new ArgumentException($"The table, {tableName}, already exists.");
            storage.InsertTable(tableName);
            schemas.Add(tableName, columns);
            indices.AddPrimaryIndex(tableName);
            indices.AddSecondaryIndex(tableName, IndexNames.IS_DELETED);
            tables.Add(tableName);
        }

        public void Delete(string tableName)
        {
            if (!exists(tableName))
                throw new ArgumentException($"The table, {tableName}, does not exist.");
            schemas.Remove(tableName);
            indices.RemoveAll(tableName);
            storage.DeleteTable(tableName);
            tables.Remove(tableName);
        }

        public ITable GetByName(string name)
        {
            if (!exists(name))
                throw new ArgumentException($"The table, {name}, does not exist.");
            return new Table(this.storage, schemas.GetSchema(name), indices, idGenerator);
        }

        public bool Exists(string name) => exists(name);

        private bool exists(string name) => tables.Contains(name);
    }
}
