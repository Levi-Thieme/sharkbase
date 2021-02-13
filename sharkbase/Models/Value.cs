﻿using System;
using System.IO;

namespace SharkBase.Models
{
    public class Value
    {
        public object value { get; set; }

        public Value(object value)
        {
            this.value = value;
        }

        public void WriteTo(BinaryWriter writer)
        {
            if (value is string stringValue)
                writer.Write(stringValue);
            else if (value is long longValue)
                writer.Write(longValue);
            else
                throw new ArgumentException("Value is not writable for the supported value types.");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            else if (obj is Value other) return this.value.Equals(other.value);
            else return false;
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}