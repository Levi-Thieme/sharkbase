using System.Collections.Generic;

namespace SharkBase.DataAccess
{
    public interface SchemaRepository
    {
        void Add(string tableName, IEnumerable<Column> columns);
        void Remove(string tableName);
        TableSchema GetSchema(string tableName);
    }
}
