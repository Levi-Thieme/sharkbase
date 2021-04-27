using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharkBase.QueryProcessing.Parsing
{
    public class UpdateRecordParser : IParser
    {
        private const string UPDATE = "UPDATE"; //update food set price = 5;

        public bool IsParsable(string input) => input.StartsWith(UPDATE);

        public IStatement Parse(string input)
        {
            string[] tokens = Parser.TokenizeStatement(string.Join(" ", input.Split("=")));
            if (tokens.Length < 5)
            {
                throw new ArgumentException("Invalid input. An update statement must conform to the following format with an optional WHERE clause: UPDATE {TABLE} SET {COLUMN} = {VALUE}");
            }
            string table = tokens[1];
            string column = tokens[3];
            string value = tokens[4].Replace("'", "");
            if (tokens.Length == 5)
            {
                return new UpdateRecordStatement(table, column, value);
            }
            else if (tokens.Length >= 8)
            {
                var whereClauseTokens = parseWhereClause(tokens.SkipWhile(token => token != "WHERE")).Skip(1).ToArray();
                return new UpdateRecordStatement(table, column, value, whereClauseTokens[0], whereClauseTokens[1]);
            }
            else
            {
                throw new ArgumentException("Invalid input. An update statement must conform to the following format with an optional WHERE clause: UPDATE {TABLE} SET {COLUMN} = {VALUE}");
            }
        }

        private IEnumerable<string> parseWhereClause(IEnumerable<string> tokens)
        {
            if (!tokens.Any())
                throw new ArgumentException("A WHERE clause must contain at least one selection criteria.");
            return Parser.GetColumnValues(tokens);
        }
    }
}
