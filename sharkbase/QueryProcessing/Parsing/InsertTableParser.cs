using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Validation;
using SharkBase.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.QueryProcessing.Parsing
{
    public class InsertTableParser : IParser
    {
        private const string INSERT_TABLE = "INSERT TABLE";

        public bool IsParsable(string input) => input.StartsWith(INSERT_TABLE);

        public IStatement Parse(string input)
        {
            string[] tokens = Parser.TokenizeStatement(input);
            CheckSyntax(tokens);
            return new InsertTableStatement(new InsertTableValidator()) { Table = tokens[2], Tokens = skipToColumnDefinitions(tokens) };
        }

        private void CheckSyntax(string[] tokens)
        {
            if (tokens.Length < 3)
            {
                throw new ArgumentException("Insert table statement is missing a table name.");
            }
            CheckColumnDefinitionSyntax(skipToColumnDefinitions(tokens).Count());
        }

        private IEnumerable<string> skipToColumnDefinitions(IEnumerable<string> tokens) => tokens.Skip(3);

        private void CheckColumnDefinitionSyntax(int columnDefinitionTokens)
        {
            if (columnDefinitionTokens < 2)
            {
                throw new ArgumentException("Insert table statement is missing at least one column definition.");
            }
            else if (columnDefinitionTokens % 2 != 0)
            {
                throw new ArgumentException("Insert table statement has a malformed column definition. Each column definition requires a type and name.");
            }
        }
    }
}
