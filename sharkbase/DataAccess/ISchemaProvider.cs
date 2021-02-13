using System.Collections.Generic;

namespace SharkBase.DataAccess
{
    public interface ISchemaProvider
    {
        void AddSchema(TableSchema table);
        void RemoveSchema(TableSchema table);
        TableSchema GetSchema(string tableName);
        IEnumerable<TableSchema> GetAllSchemas();
    }
}
