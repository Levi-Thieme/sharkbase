using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.QueryProcessing.Parsing
{
    public class InsertRecordParser : IParser
    {
        private const string INSERT_INTO = "INSERT INTO";

        public bool IsParsable(string input) => input.StartsWith(INSERT_INTO);

        public IStatement Parse(string input)
        {
            throw new NotImplementedException();
        }
    }
}
