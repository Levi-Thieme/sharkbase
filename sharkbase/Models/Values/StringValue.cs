using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.Models.Values
{
    public class StringValue : Value
    {
        private string value = string.Empty;
        public string Value { 
            get { return value.Substring(0, length); }
            private set { this.value = value; }
        }
        public int length { get; private set; }

        public StringValue()
        {
            value = string.Empty;
            length = 0;
        }

        public StringValue(string value)
        {
            this.value = value;
            length = value.Length;
        }

        public void Read(BinaryReader reader)
        {
            this.length = reader.ReadInt32();
            this.value = reader.ReadString();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(length);
            writer.Write(value);
        }

        public override string ToString()
        {
            return this.value;
        }

        public override bool Equals(object obj)
        {
            return obj is StringValue other && this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}
