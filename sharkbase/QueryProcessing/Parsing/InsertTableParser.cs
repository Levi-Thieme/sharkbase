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
            return new InsertTableStatement(new InsertTableValidator(), tokens[2], parseColumnDefinitions(skipToColumnDefinitions(tokens)));
        }

        private void CheckSyntax(string[] tokens)
        {
            if (tokens.Length < 3)
            {
                throw new ArgumentException("Insert table statement is missing a table name.");
            }
            containsColumnDefinitions(skipToColumnDefinitions(tokens).Count());
        }

        private IEnumerable<string> skipToColumnDefinitions(IEnumerable<string> tokens) => tokens.Skip(3);

        private void containsColumnDefinitions(int columnDefinitionTokens)
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

        private IEnumerable<Column> parseColumnDefinitions(IEnumerable<string> tokens)
        {
            List<Column> columns = new List<Column>();
            var columnDefinitions = tokens.ToList();
            for (int i = 0; i < tokens.Count(); i += 2)
            {
                string columnName = columnDefinitions[i];
                string columnType = columnDefinitions[i + 1];
                columns.Add(parseColumn(columnName, columnType));
            }

            return columns;
        }

        private Column parseColumn(string name, string type)
        {
            if (ColumnTypes.Exists(type))
            {
                return new Column(ColumnTypes.ColumnTypeByName[type], name);
            }
            else if (type.ToUpper().StartsWith(ColumnTypes.StringType))
            {
                try
                {
                    int start = type.IndexOf("(") + 1;
                    int substringLength = type.IndexOf(")", start + 1) - start;
                    string length = type.Substring(start, substringLength);
                    return new Column(DataTypes.String, name, size: int.Parse(length));
                }
                catch (Exception)
                {
                    throw new ArgumentException($"Unable to parse column definition for the column, {name}, given type {type}");
                }
            }
            throw new ArgumentException($"Unable to parse column definition for the column, {name}, given type {type}");
        }
    }
}
