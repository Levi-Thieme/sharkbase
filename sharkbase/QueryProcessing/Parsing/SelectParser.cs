using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.QueryProcessing.Parsing
{
    public class SelectParser : IParser
    {
        private const string SELECT_FROM = "SELECT FROM";
        private readonly SchemaRepository schemaProvider;

        public bool IsParsable(string input) => input.StartsWith(SELECT_FROM);

        public SelectParser(SchemaRepository schemaProvider)
        {
            this.schemaProvider = schemaProvider;
        }

        public IStatement Parse(string input)
        {
            string[] tokens = Parser.TokenizeStatement(string.Join(" ", input.Split("=")));
            var whereClauseTokens = new List<string>();
            if (tokens.Length < 3)
                throw new ArgumentException("A table name must be provided for the select query.");
            else if (tokens.Length > 3 && tokens[3] == "WHERE")
            {
                whereClauseTokens = parseWhereClause(tokens.Skip(4)).ToList();
                whereClauseTokens.RemoveAll(token => token == "=");
                whereClauseTokens = whereClauseTokens.SelectMany(token => token.Split("=")).Where(token => !string.IsNullOrEmpty(token?.Trim())).ToList();
            }
            return new SelectStatement(new SelectValidator(this.schemaProvider), tokens[2], whereClauseTokens);
        }

        private IEnumerable<string> parseWhereClause(IEnumerable<string> tokens)
        {
            if (!tokens.Any())
                throw new ArgumentException("A WHERE clause must contain at least one selection criteria.");
            return Parser.GetColumnValues(tokens);
        }
    }
}