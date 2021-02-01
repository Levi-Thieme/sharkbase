using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.Parsing
{
    public class ValueParser
    {
        public long ParseInt(string value)
        {
            return long.Parse(value);
        }

        public string ParseString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            else if (value.Length > 128)
                return value.Substring(0, 128);
            return value.Trim();
        }
    }
}
