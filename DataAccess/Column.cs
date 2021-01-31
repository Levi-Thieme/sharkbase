using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public class Column
    {
        public readonly ColumnType Type;
        public readonly string Name;

        public Column(ColumnType type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Column)
            {
                var other = (Column)obj;
                return this.Type == other.Type &&
                    this.Name == other.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode() + this.Name.GetHashCode();
        }
    }
}
