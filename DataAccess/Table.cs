using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public class Table : ITable
    {
        private ISystemStore store;
        private string name;

        public Table(ISystemStore store, string name)
        {
            this.store = store;
            this.name = name;
        }

        public void InsertRecord(IEnumerable<object> values)
        {
            throw new NotImplementedException();
        }
    }
}
