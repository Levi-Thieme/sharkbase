using SharkBase.DataAccess;
using SharkBase.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.QueryProcessing.Validation
{
    public class ColumnValueValidator
    {
        public void ValidateColumnValues(TableSchema schema, IEnumerable<string> columnValueTokens)
        {
            var tokens = columnValueTokens.ToList();
            if (tokens.Count() != 2)
                throw new ArgumentException("Invalid number of tokens. Tokens must contain a column name and value.");

            var column = schema.Columns.FirstOrDefault(c => c.Name == tokens.ElementAt(0));
            if (column == null)
                throw new ArgumentException($"The column, {tokens.ElementAt(0)}, does not exist in the table {schema.Name}");
            try
            {
                new ValueParser().ParseValue(tokens.ElementAt(1), column);
            }
            catch (FormatException)
            {
                throw new ArgumentException($"The value, {tokens.ElementAt(1)}, could not be parsed for the column {column.Name} with type {column.Type}");
            }
        }
    }
}
