using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.SystemStorage
{
    public interface ISystemStore
    {
        void InsertTable(string name, IEnumerable<Column> columns);
        void DeleteTable(string name);
    }
}
