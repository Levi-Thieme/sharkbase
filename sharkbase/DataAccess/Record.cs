using SharkBase.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharkBase.DataAccess
{
    public class Record
    {
        public IEnumerable<Value> Values { get; set; } = new List<Value>();

        public Record(IEnumerable<Value> values)
        {
            Values = values;
        }

        public Record(List<Value> values)
        {
            Values = values;
        }

        public void WriteTo(BinaryWriter writer)
        {
            foreach (var value in Values)
                value.WriteTo(writer);
        }

        public string GetId() => Values.Any() ? Values.First().ToString() : string.Empty;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            else if (obj is Record other)
            {
                return this.Values
                    .Zip<Value, Value, bool>
                    (other.Values, (thisValue, otherValue) => thisValue.Equals(otherValue))
                    .All(boolean => boolean);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return this.Values.Select(value => value.GetHashCode()).Sum();
        }

        public override string ToString() => string.Join(" | ", Values.Select(v => v.ToString()));
    }
}
