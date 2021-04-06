using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.QueryProcessing.Validation
{
    public class UpdateRecordValidator : IStatementValidator, IValidate<UpdateRecordStatement>
    {
        private SchemaRepository schemas;

        public UpdateRecordValidator(SchemaRepository @object)
        {
            this.schemas = @object;
        }

        public void Validate(string table, IEnumerable<string> tokens)
        {
            throw new NotImplementedException();
        }

        public void Validate(UpdateRecordStatement statement)
        {
            var schema = schemas.GetSchema(statement.Table);
            if (schema == null)
            {
                throw new ArgumentException($"The table, {statement.Table}, does not exist.");
            }
            validateColumnAndValue(schema, statement.Column, statement.Value);
            if (!string.IsNullOrEmpty(statement.WhereColumn) && !string.IsNullOrEmpty(statement.WhereColumnValue))
            {
                validateColumnAndValue(schema, statement.WhereColumn, statement.WhereColumnValue);
            }
        }

        private void validateColumnAndValue(TableSchema schema, string columnName, string value)
        {
            var column = schema.Columns.FirstOrDefault(c => c.Name == columnName);
            var valueParser = new ValueParser();
            if (column == null)
            {
                throw new ArgumentException($"The column, {column}, does not exist");
            }
            else
            {
                try
                {
                    valueParser.ParseValue(value, column.Type);
                }
                catch (Exception)
                {
                    throw new ArgumentException($"The given value, {value}, is not parsable for the column type.");
                }
            }
        }
    }
}
