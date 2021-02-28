using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.DataAccess.Schema.Repositories
{
    public class FileSchemas : SchemaRepository
    {
        PhysicalStorage store;

        public FileSchemas(PhysicalStorage store)
        {
            this.store = store;
        }

        public void AddSchema(TableSchema table)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TableSchema> GetAllSchemas()
        {
            throw new NotImplementedException();
        }

        public TableSchema GetSchema(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
