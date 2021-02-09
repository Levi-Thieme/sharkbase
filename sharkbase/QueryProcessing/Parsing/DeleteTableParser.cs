using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System;

namespace SharkBase.QueryProcessing.Parsing
{
    public class DeleteTableParser : IParser
    {
        private const string DELETE_TABLE = "DELETE TABLE";

        public bool IsParsable(string input) => input.StartsWith(DELETE_TABLE);

        public IStatement Parse(string input)
        {
            string[] tokens = Parser.TokenizeStatement(input);
            checkSyntax(tokens);
            return new DeleteTableStatement(tokens[2], new DeleteTableValidator());
        }

        private void checkSyntax(string[] tokens)
        {
            if (tokens.Length < 3)
                throw new ArgumentException("The delete table statement is missing a table name.");
            else if (tokens.Length > 4)
                throw new ArgumentException("The delete table statement contains more than a table name.");
        }
    }
}
