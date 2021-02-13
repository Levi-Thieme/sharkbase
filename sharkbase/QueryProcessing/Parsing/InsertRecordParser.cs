using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.QueryProcessing.Parsing
{
    public class InsertRecordParser : IParser
    {
        private const string INSERT_INTO = "INSERT INTO";
        private ISchemaProvider schemaProvider;

        public bool IsParsable(string input) => input.StartsWith(INSERT_INTO);

        public InsertRecordParser(ISchemaProvider schemaProvider)
        {
            this.schemaProvider = schemaProvider;
        }

        public IStatement Parse(string input)
        {
            string[] tokens = Parser.TokenizeStatement(input);
            if (tokens.Length < 3)
                throw new ArgumentException("An insert record statement requires a table name.");
            var columnValues = GetColumnValues(tokens.Skip(3).ToArray());
            return new InsertRecordStatement(tokens[2], columnValues, new InsertRecordValidator(this.schemaProvider));
        }

        public IEnumerable<string> GetColumnValues(string[] tokens)
        {
            var values = new List<string>();
            bool inAString = false;
            int startIndexOfString = 0;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].StartsWith("'"))
                {
                    if (tokens[i].EndsWith("'"))
                    {
                        values.Add(removeSingleQuotes(tokens[i]));
                    }
                    else
                    {
                        inAString = true;
                        startIndexOfString = i;
                    }
                }
                else if (tokens[i].EndsWith("'"))
                {
                    if (!inAString)
                        throw new ArgumentException("A string literal in the insert statement is missing an opening single quote.");
                    var stringTokens = selectTokensFromTo(tokens, startIndexOfString, i).ToList();
                    values.Add(joinTokensWithSingleQuotesRemoved(stringTokens));
                    inAString = false;
                }
                else if (!inAString)
                {
                    values.Add(tokens[i]);
                }
            }
            if (inAString)
            {
                throw new ArgumentException("A string literal in the insert statement was not closed with a single quote.");
            }
            return values;
        }

        private IEnumerable<string> selectTokensFromTo(string[] tokens, int startIndex, int currentIndex)
        {
            return tokens
                .Skip(startIndex)
                .Take((currentIndex - startIndex) + 1);
        }

        private string joinTokensWithSingleQuotesRemoved(IEnumerable<string> tokens) {
            string theString = string.Join(" ", tokens);
            return removeSingleQuotes(theString);
        }

        private string removeSingleQuotes(string token) => token.Substring(1, token.Length - 2);
    }
}
