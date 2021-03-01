using Newtonsoft.Json;
using SharkBase.Startup;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharkBase.DataAccess.Schema.Repositories
{
    public class FileSchemas : SchemaRepository
    {
        private readonly PhysicalStorage store;
        private readonly IEnumerable<Column> DefaultColumns = new List<Column>
        {
            new Column(ColumnType.String, "ID", hasDefaultValue: true),
            new Column(ColumnType.boolean, "DELETED", hasDefaultValue: true),
        };


        public FileSchemas(PhysicalStorage store)
        {
            this.store = store;
        }

        public void Add(string name, IEnumerable<Column> columns)
        {
            var schemaColumns = DefaultColumns.ToList();
            schemaColumns.AddRange(columns);
            var schema = new TableSchema(name, schemaColumns);
            using (var stream = store.GetOverwritingSchemaStream(name))
            {
                string schemaJson = JsonConvert.SerializeObject(schema);
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(schemaJson);
                }
            }   
        }

        public void Remove(string name)
        {
            store.DeleteSchema(name);
        }

        public TableSchema GetSchema(string tableName)
        {
            using (var stream = store.GetSchemaStream(tableName))
            {
                using (var reader = new BinaryReader(stream))
                {
                    string schemaJson = reader.ReadString();
                    return JsonConvert.DeserializeObject<TableSchema>(schemaJson);
                }
            }    
        }
    }
}
