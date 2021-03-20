using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.Models.Values
{
    public class LongValue : Value
    {
        private long value;

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
            return obj is long other && this.value == other;
        }
    }
}
