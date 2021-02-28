using System.Collections.Generic;
using System.IO;

namespace SharkBase.DataAccess
{
    public interface SchemaRepository
    {
        void AddSchema(TableSchema table);
        TableSchema GetSchema(string tableName);
        IEnumerable<TableSchema> GetAllSchemas();
    }
}
