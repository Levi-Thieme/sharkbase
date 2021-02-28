using SharkBase.DataAccess;
using System.Linq;
using System.Collections.Generic;
using System;
using SharkBase.Parsing;

namespace SharkBase.QueryProcessing.Validation
{
    public class SelectValidator : IStatementValidator
    {
        private SchemaRepository schemaProvider;

        public SelectValidator(SchemaRepository schemaProvider)
        {
            this.schemaProvider = schemaProvider;
        }

        public void Validate(string table, IEnumerable<string> tokens)
        {
            var schema = schemaProvider.GetSchema(table);
            if (schema == null)
                throw new ArgumentException($"The table, {table}, does not exist.");
            if (tokens.Any())
            {
                new ColumnValueValidator().ValidateColumnValues(schema, tokens); 
            }  
        }
    }
}
