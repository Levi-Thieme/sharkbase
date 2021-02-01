using System;
using System.Collections.Generic;
using System.Linq;
using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Parsing;

namespace SharkBase.Parsing
{
    public class Parser : IParser
    {
        private IEnumerable<IParser> parsers;
        private Dictionary<string, ColumnType> ColumnTypes = new Dictionary<string, ColumnType>
        {
            { "INT64", ColumnType.Int64 },
            { "CHAR128", ColumnType.Char128 }
        };

        public Parser (IEnumerable<IParser> parsers)
        {
            this.parsers = parsers;
        }

        public bool IsParsable(string input) => !string.IsNullOrEmpty(input);

        public IStatement Parse(string input)
        {
            input = input.Trim();
            if (IsParsable(input))
            {
                var parser = parsers.FirstOrDefault(p => p.IsParsable(input));
                if (parser == null)
                {
                    throw new ArgumentException("The input is not a recognized command.");
                }
                return parser.Parse(input);
            }
            throw new ArgumentException("The input is not a recognized command.");
        }

        public static string[] TokenizeStatement(string statement)
        {
            const string delimiter = " ";
            string[] tokens = statement.Split(delimiter).Where(token => !string.IsNullOrEmpty(token.Trim())).ToArray();
            return tokens;
        }
    }
}
