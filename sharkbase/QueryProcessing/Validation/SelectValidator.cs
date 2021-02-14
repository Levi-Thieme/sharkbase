using SharkBase.DataAccess;
using System.Linq;
using System.Collections.Generic;
using System;
using SharkBase.Parsing;

namespace SharkBase.QueryProcessing.Validation
{
    public class SelectValidator : IStatementValidator
    {
        private ISchemaProvider schemaProvider;

        public SelectValidator(ISchemaProvider schemaProvider)
        {
            this.schemaProvider = schemaProvider;
        }

        public void Validate(string table, IEnumerable<string> tokens)
        {
            if (tokens.Any())
            {
                if (tokens.Count() != 2)
                    throw new ArgumentException("Invalid number of tokens. Tokens must contain a column name and value.");
                var schema = schemaProvider.GetSchema(table);
                if (schema == null)
                    throw new ArgumentException($"The table, {table}, does not exist.");
                var column = schema.Columns.FirstOrDefault(c => c.Name == tokens.ElementAt(0));
                if (column == null)
                    throw new ArgumentException($"The column, {tokens.ElementAt(0)}, does not exist in the table {schema.Name}");
                try
                {
                    new ValueParser().ParseValue(tokens.ElementAt(1), column.Type);
                }
                catch (FormatException)
                {
                    throw new ArgumentException($"The value, {tokens.ElementAt(1)}, could not be parsed for the column {column.Name} with type {column.Type}");
                }
            }  
        }
    }
}
