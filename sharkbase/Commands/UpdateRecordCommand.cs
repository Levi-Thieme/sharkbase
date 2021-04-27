using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharkBase.Commands
{
    public class UpdateRecordCommand : ICommand
    {
        private UpdateRecordStatement statement;
        private ITable table;

        public UpdateRecordCommand(UpdateRecordStatement updateRecordStatement, ITable table)
        {
            this.statement = updateRecordStatement;
            this.table = table;
        }

        public void Execute()
        {
            var updatedRecords = new List<Record>();
            using (var recordStream = table.ReadAll())
            {
                if (string.IsNullOrEmpty(statement.WhereColumn))
                {
                    foreach (var record in recordStream)
                    {
                        int updateColumnIndex = table.Schema.Columns.ToList().FindIndex(c => c.Name == statement.Column);
                        var updatedValues = record.Values.ToArray();
                        updatedValues[updateColumnIndex] = new ValueParser().ParseValue(statement.Value, table.Schema.Columns.ElementAt(updateColumnIndex));
                        var updatedRecord = new Record(updatedValues);
                        updatedRecords.Add(updatedRecord);
                    }
                }
                else
                {
                    int whereColumnIndex = table.Schema.Columns.ToList().FindIndex(c => c.Name == statement.WhereColumn);
                    var whereColumn = table.Schema.Columns.ElementAt(whereColumnIndex);

                    foreach (var record in recordStream)
                    {
                        var valueParser = new ValueParser();
                        var whereColumnValue = valueParser.ParseValue(statement.WhereColumnValue, whereColumn);
                        if (record.Values.ElementAt(whereColumnIndex).Equals(whereColumnValue))
                        {
                            int updateColumnIndex = table.Schema.Columns.ToList().FindIndex(c => c.Name == statement.Column);
                            var updatedValues = record.Values.ToArray();
                            updatedValues[updateColumnIndex] = valueParser.ParseValue(statement.Value, table.Schema.Columns.ElementAt(updateColumnIndex));
                            var updatedRecord = new Record(updatedValues);
                            updatedRecords.Add(updatedRecord);
                        }
                    }
                }
            }
            table.UpdateRecords(updatedRecords);
        }
    }
}
