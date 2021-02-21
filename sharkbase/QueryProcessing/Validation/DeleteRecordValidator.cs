using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharkBase.QueryProcessing.Validation
{
    public class DeleteRecordValidator : IStatementValidator
    {
        private ISchemaProvider schemaProvider;
        
        public DeleteRecordValidator(ISchemaProvider schemaProvider)
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
