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

        public Command Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException();
            var tokens = input.Split(" ");
            var command = new Command(CommandTypes[tokens[0]], tokens[2]);
            command.Columns = ParseColumns(input);
            return command;
        }

        private Dictionary<string, ColumnType> ParseColumns(string input)
        {
            var columnDictionary = new Dictionary<string, ColumnType>();
            int columnsIndex = input.IndexOf("COLUMNS ");
            if (columnsIndex == -1)
                return columnDictionary;
            string columns = input.Substring(columnsIndex + "COLUMNS ".Length);
            var columnsWithType = columns.Split(",");
            
            foreach (string columnWithType in columnsWithType)
            {
                var columnAndType = columnWithType.Split(":");
                columnDictionary.Add(columnAndType[0], ColumnTypes[columnAndType[1]]);
            }
            return columnDictionary;
        }
    }
}
