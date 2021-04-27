using SharkBase.Models;
using SharkBase.Models.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharkBase.DataAccess
{
    public class Record : Writable
    {
        public IEnumerable<Value> Values { get; private set; }

        public Record(IEnumerable<Value> values)
        {
            Values = values;
        }

        public Record(Record updatedRecord)
        {
            this.Values = updatedRecord.Values.ToArray();
        }

        public Record(List<Value> values)
        {
            Values = values;
        }

        public void Write(BinaryWriter writer)
        {
            foreach (var value in Values)
                value.Write(writer);
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
            string id = GetId();
            return string.IsNullOrEmpty(id) ? Values.Select(v => v.GetHashCode()).Sum() % int.MaxValue : id.GetHashCode();
        }

        public override string ToString() => string.Join(" | ", Values.Select(v => v.ToString()));
    }
}
