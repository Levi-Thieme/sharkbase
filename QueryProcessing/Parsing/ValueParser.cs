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
            return value.Substring(0, 128);
        }
    }
}
