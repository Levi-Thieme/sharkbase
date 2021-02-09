using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public class TableSchema
    {
        public string Name { get; private set; }
        public IEnumerable<Column> Columns { get; private set; }

        public TableSchema(string name, IEnumerable<Column> columns)
        {
            this.Name = name;
            this.Columns = columns;
        }

        public TableSchema(string name)
        {
            this.Name = name;
            this.Columns = new List<Column>();
        }
    }
}
