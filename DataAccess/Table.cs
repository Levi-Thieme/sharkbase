using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public class Table
    {
        public string Name { get; private set; }
        public IEnumerable<Column> Columns { get; private set; }

        public Table(string name, IEnumerable<Column> columns)
        {
            this.Name = name;
            this.Columns = columns;
        }

        public Table(string name)
        {
            this.Name = name;
            this.Columns = new List<Column>();
        }
    }
}
