using System;
using System.Collections.Generic;
using System.Text;
using SharkBase.Commands;
using System.Linq;
using SharkBase.DataAccess;

namespace SharkBase.Parsing
{
    public class Parser
    {
        private Dictionary<string, CommandType> CommandTypes = new Dictionary<string, CommandType> 
        {
            { "INSERT", CommandType.Insert },
            { "DELETE", CommandType.Delete },
            { "UPDATE", CommandType.Update }
        };

        private Dictionary<string, ColumnType> ColumnTypes = new Dictionary<string, ColumnType>
        {
            { "INT64", ColumnType.Int64 },
            { "CHAR128", ColumnType.Char128 }
        };

        private const string INSERT_TABLE = "INSERT TABLE";
        private const string DELETE_TABLE = "DELETE TABLE";
        private const string INSERT_INTO = "INSERT INTO";

        private Dictionary<string, CommandType> StatementTypes = new Dictionary<string, CommandType>
        {
            { INSERT_TABLE, CommandType.Insert }
        };

        public TableCommand Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException();
            if (input.StartsWith(INSERT_TABLE))
            {
                return ParseInsertTableStatement(input);
            }
            else if (input.StartsWith(DELETE_TABLE))
            {
                return ParseDeleteTableStatement(input);
            }
            else if (input.StartsWith(INSERT_INTO))
            {
                return ParseInsertRecordStatement(input);
            }
            throw new ArgumentException("The command type could not be parsed.");
        }

        private static string[] TokenizeStatement(string statement)
        {
            const string delimiter = " ";
            string[] tokens = statement.Split(delimiter).Where(token => !string.IsNullOrEmpty(token.Trim())).ToArray();
            return tokens;
        }

        private TableCommand ParseInsertTableStatement(string statement)
        {
            string[] tokens = TokenizeStatement(statement);
            var columns = ParseColumns(tokens.Skip(3));
            return new TableCommand(CommandType.Insert, tokens[2], columns);
        }

        private TableCommand ParseDeleteTableStatement(string statement)
        {
            string[] tokens = TokenizeStatement(statement);
            if (tokens.Length != 3)
                throw new ArgumentException("DELETE TABLE statements must specify a table name.");
            return new TableCommand(CommandType.Delete, tokens[2]);
        }

        private TableCommand ParseInsertRecordStatement(string statement)
        {
            string[] tokens = TokenizeStatement(statement);
            if (tokens.Length < 3)
                throw new ArgumentException("INSERT RECORD statements must specify a table name.");
            var columns = ParseColumns(tokens.Skip(3));
            return new TableCommand(CommandType.InsertRecord, tokens[2], columns);
        }

        private IEnumerable<Column> ParseColumns(IEnumerable<string> columns)
        {
            var parsedColumns = new List<Column>();
            var columnList = columns.ToList();
            if (columns.Count() == 0 || columns.Count() % 2 != 0)
            {
                throw new ArgumentException("Columns must have type and name delimited by a whitespace.");
            }
            for (int i = 0; i < columnList.Count; i += 2)
            {
                string type = columnList[i];
                string name = columnList[i + 1];
                parsedColumns.Add(new Column(ColumnTypes[type], name));
            }
            return parsedColumns;
        }
    }
}
