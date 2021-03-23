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
        private SchemaRepository schemaProvider;

        public bool IsParsable(string input) => input.StartsWith(INSERT_INTO);

        public InsertRecordParser(SchemaRepository schemaProvider)
        {
            this.schemaProvider = schemaProvider;
        }

        public IStatement Parse(string input)
        {
            string[] tokens = Parser.TokenizeStatement(input);
            if (tokens.Length < 3)
                throw new ArgumentException("An insert record statement requires a table name.");
            string tableName = tokens[2];
            var columnValues = Parser.GetColumnValues(tokens.Skip(3));
            var updatedColumnValues = truncateStrings(schemaProvider.GetSchema(tableName).Columns.Where(c => !c.HasDefaultValue), columnValues);
            return new InsertRecordStatement(tableName, updatedColumnValues, new InsertRecordValidator(this.schemaProvider));
        }

        private IEnumerable<string> truncateStrings(IEnumerable<Column> columns, IEnumerable<string> columnValues)
        {
            var updatedValues = columnValues.ToList();
            for (int i = 0; i < columns.Count(); i++)
            {
                var column = columns.ElementAt(i);
                if (columns.ElementAt(i).Type == DataTypes.String)
                {
                    updatedValues[i] = updatedValues[i].Length > column.Size ? updatedValues[i].Substring(0, column.Size) : updatedValues[i];
                }
            }
            return updatedValues;
        }
        
    }
}
