using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.Models.Values
{
    public class StringValue : Value
    {
        public string value { get; private set; }

        public StringValue()
        {
            value = string.Empty;
        }

        public StringValue(string value)
        {
            this.value = value;
        }

        public void Read(BinaryReader reader)
        {
            this.value = reader.ReadString();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(value);
        }

        public override string ToString()
        {
            return this.value;
        }

        public override bool Equals(object obj)
        {
            return obj is StringValue other && this.value.Trim() == other.value.Trim();
        }
    }
}
