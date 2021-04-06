using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.Models.Values
{
    public class LongValue : Value
    {
        public long value { get; private set; }

        public LongValue() { }

        public LongValue(long value)
        {
            this.value = value;
        }

        public void Read(BinaryReader reader)
        {
            this.value = reader.ReadInt64();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(value);
        }

        public override string ToString()
        {
            return this.value.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is LongValue other && this.value == other.value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
