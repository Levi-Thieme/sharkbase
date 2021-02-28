using SharkBase.DataAccess.Index;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IEnumerable<Column> DefaultColumns = new List<Column>
        {
            new Column(ColumnType.String, "ID", hasDefaultValue: true),
            new Column(ColumnType.boolean, "DELETED", hasDefaultValue: true),
        };

        public Tables(PhysicalStorage storage, IGenerateId idGenerator, SchemaRepository schemas, IndexRepository indices, IEnumerable<string> tables)
        {
            this.storage = storage;
            this.idGenerator = idGenerator;
            this.schemas = schemas;
            this.indices = indices;
            this.tables = tables.ToList();
        }

        public void Create(string name, IEnumerable<Column> columns)
        {
            if (exists(name))
                throw new ArgumentException($"The table, {name}, already exists.");
            var schemaColumns = DefaultColumns.ToList();
            schemaColumns.AddRange(columns);
            schemas.AddSchema(new TableSchema(name, schemaColumns));
            indices.Upsert(new PrimaryIndex(name, new Dictionary<string, long>()));
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
            return new Table(this.storage, schemas.GetSchema(name), indices.Get(name), idGenerator);
        }

        public bool Exists(string name) => exists(name);

        private bool exists(string name) => tables.Contains(name);
    }
}
