using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public interface ISchemaProvider
    {
        void AddSchema(TableSchema table);
        void RemoveSchema(TableSchema table);
        TableSchema GetSchema(string tableName);
    }
}
