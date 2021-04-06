using System.IO;

namespace SharkBase.Models.Values
{
    public class StringValue : Value
    {
        private string value = string.Empty;
        public string Value { 
            get { return value.Substring(0, Length); }
            private set { this.value = value; }
        }
        public int Length { get; private set; }
        public int MaxLength { get; private set; }

        public StringValue(string value)
        {
            this.value = value;
            Length = value.Length;
            MaxLength = value.Length;
        }

        public StringValue(string value, int maxLength)
        {
            this.value = value;
            Length = value.Length;
            MaxLength = maxLength;
        }

        public StringValue()
        {
            value = string.Empty;
            Length = 0;
            MaxLength = 0;
        }

        public void Read(BinaryReader reader)
        {
            this.Length = reader.ReadInt32();
            this.value = reader.ReadString();
            this.MaxLength = this.value.Length;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Length);
            writer.Write(Value.PadRight(MaxLength));
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
