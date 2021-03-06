﻿using SharkBase.DataAccess;
using System.Linq;
using System.Collections.Generic;
using System;
using SharkBase.Parsing;

namespace SharkBase.QueryProcessing.Validation
{
    public class InsertRecordValidator : IStatementValidator
    {
        private readonly SchemaRepository schemaProvider;

        public InsertRecordValidator(SchemaRepository schemaProvider)
        {
            this.schemaProvider = schemaProvider;
        }

        public void Validate(string table, IEnumerable<string> tokens)
        {
            var schema = schemaProvider.GetSchema(table);
            if (schema.Columns.Where(c => !c.HasDefaultValue).Count() != tokens.Count())
                throw new ArgumentException("The insert statement's number of columns does not match the table's column count.");
            var valueParser = new ValueParser();
            valueParser.ParseColumnValues(tokens, schema.Columns.Where(c => !c.HasDefaultValue));
        }
    }
}
