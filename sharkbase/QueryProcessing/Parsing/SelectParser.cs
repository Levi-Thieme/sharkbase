using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System;

namespace SharkBase.QueryProcessing.Parsing
{
    public class SelectParser : IParser
    {
        private const string SELECT_FROM = "SELECT FROM";

        public bool IsParsable(string input) => input.StartsWith(SELECT_FROM);

        public IStatement Parse(string input)
        {
            string[] tokens = Parser.TokenizeStatement(input);
            if (tokens.Length < 3)
                throw new ArgumentException("A table name must be provided for the select query.");
            return new SelectStatement(new AlwaysValidValidator()) { Table = tokens[2] };
        }
    }
}
