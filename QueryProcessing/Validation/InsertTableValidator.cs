using SharkBase.DataAccess;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SharkBase.QueryProcessing.Validation
{
    public class InsertTableValidator : IStatementValidator
    {
        private readonly ITables tables;

        public InsertTableValidator(ITables tables)
        {
            this.tables = tables;
        }

        public void Validate(string table, IEnumerable<string> tokens)
        {
            if (tables.Exists(table))
            {
                throw new ArgumentException($"The table, {table}, already exists.");
            }
            ValidateColumnDefinitions(tokens);
        }

        private void ValidateColumnDefinitions(IEnumerable<string> columnDefinitions)
        {
            var columnNames = new List<string>();
            for (int i = 0; i < columnDefinitions.Count(); i += 2)
            {
                if (!ColumnTypes.Exists(columnDefinitions.ElementAt(i)))
                    throw new ArgumentException($"The type, {columnDefinitions.ElementAt(i)}, is not a valid column type.");
                if (columnNames.Contains(columnDefinitions.ElementAt(i + 1)))
                    throw new ArgumentException($"A table's column names must be unique. {columnDefinitions.ElementAt(i + 1)} is already defined.");
                columnNames.Add(columnDefinitions.ElementAt(i + 1));
            }
        }
    }
}
