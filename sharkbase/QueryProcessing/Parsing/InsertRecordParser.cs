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
            return new InsertRecordStatement(tableName, columnValues, new InsertRecordValidator(this.schemaProvider));
        } 
    }
}
