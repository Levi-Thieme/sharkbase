using SharkBase.Commands;
using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharkBase.Parsing;

namespace SharkBase.QueryProcessing.Validation
{
    public class InsertRecordValidator : IValidateInsertRecordCommand
    {
        public void Validate(InsertRecordCommand command, Table table)
        {
            var valueParser = new ValueParser();
            if (command.ColumnValues.Count() != table.Columns.Count())
            {
                throw new ArgumentException("The number of the command's column values are not equal to the table's number of columns.");
            }
            foreach (var columnValue in command.ColumnValues)
            {
                var tableColumn = table.Columns.FirstOrDefault(column => column.Name == columnValue.Column);
                if (tableColumn == null)
                {
                    throw new ArgumentException($"The column, {columnValue.Column}, was not found in the table {table.Name}.");
                }
                if (tableColumn.Type == ColumnType.Int64)
                {
                    columnValue.Value = valueParser.ParseInt((string)columnValue.Value);
                }
            }
        }
    }
}
