using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public interface ISchemaProvider
    {
        void AddSchema(Table table);
        void RemoveSchema(Table table);
        Table GetSchema(string tableName);
    }
}
