﻿using System;
using System.Collections.Generic;
using System.Linq;
using SharkBase.QueryProcessing.Parsing;

namespace SharkBase.Parsing
{
    public class Parser : IParser
    {
        private readonly IEnumerable<IParser> parsers;

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

        public static IEnumerable<string> GetColumnValues(IEnumerable<string> columnValueTokens)
        {
            var values = new List<string>();
            bool inAString = false;
            int startIndexOfString = 0;
            var tokens = columnValueTokens.ToArray();
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

        public static IEnumerable<string> selectTokensFromTo(IEnumerable<string> tokens, int startIndex, int currentIndex)
        {
            return tokens
                .Skip(startIndex)
                .Take((currentIndex - startIndex) + 1);
        }

        public static string joinTokensWithSingleQuotesRemoved(IEnumerable<string> tokens)
        {
            string theString = string.Join(" ", tokens);
            return removeSingleQuotes(theString);
        }

        public static string removeSingleQuotes(string token) => token.Substring(1, token.Length - 2);
    }
}
