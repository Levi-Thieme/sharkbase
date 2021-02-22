using System.Collections.Generic;
using System.Linq;

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

        public override bool Equals(object obj)
        {
            if (obj is TableSchema other)
            {
                return this.Name == other.Name && Enumerable.SequenceEqual(this.Columns, other.Columns);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() + this.Columns.GetHashCode();
        }
    }
}
