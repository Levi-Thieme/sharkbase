using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.Models.Values
{
    public class BoolValue : Value
    {
        public bool value { get; private set; }

        public BoolValue() { }

        public BoolValue(bool value)
        {
            this.value = value;
        }

        public void Read(BinaryReader reader)
        {
            this.value = reader.ReadBoolean();
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
            return obj is BoolValue other && this.value == other.value;
        }
    }
}
