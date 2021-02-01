using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;

namespace SharkBase.DataAccess
{
    public class Tables : ITables
    {
        private ISystemStore storage;

        public Tables(ISystemStore storage)
        {
            this.storage = storage;
        }

        public void Create(string name, IEnumerable<Column> columns)
        {
            storage.InsertTable(name, columns);
        }

        public void Delete(string name)
        {
            storage.DeleteTable(name);
        }

        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }
    }
}
