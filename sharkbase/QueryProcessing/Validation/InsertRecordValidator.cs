using SharkBase.DataAccess;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using SharkBase.Parsing;

namespace SharkBase.QueryProcessing.Validation
{
    public class InsertRecordValidator : IStatementValidator
    {
        private readonly ISchemaProvider schemaProvider;

        public InsertRecordValidator(ISchemaProvider schemaProvider)
        {
            this.schemaProvider = schemaProvider;
        }

        public void Validate(string table, IEnumerable<string> tokens)
        {
            var schema = schemaProvider.GetSchema(table);
            if (schema.Columns.Count() != tokens.Count())
                throw new ArgumentException("The insert statement's number of columns does not match the table's column count.");
            var valueParser = new ValueParser();
            valueParser.ParseColumnValues(tokens, schema.Columns);
        }
    }
}
