using SharkBase.DataAccess;
using SharkBase.Models;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using System.Linq;

namespace SharkBase.Commands
{
    public class DeleteRecordCommand : ICommand
    {
        private DeleteRecordStatement statement;
        private ITable table;

        public DeleteRecordCommand(DeleteRecordStatement deleteRecordStatement, ITable table)
        {
            this.statement = deleteRecordStatement;
            this.table = table;
        }

        public void Execute()
        {
            if (statement.Tokens.Any())
            {
                using (var recordStream = table.ReadAll())
                {
                    int columnIndex = table.Schema.Columns.ToList().FindIndex(c => c.Name == statement.Tokens.First());
                    var type = table.Schema.Columns.ElementAt(columnIndex).Type;
                    var value = new ValueParser().ParseValue(statement.Tokens.ElementAt(1), type);
                    foreach (var record in recordStream)
                    {
                        if (record.Values.ElementAt(columnIndex).Equals(value))
                        {
                            table.DeleteRecord(record);
                        }
                    }
                }
            }
            else
            {
                table.DeleteAllRecords();
            }
        }
    }
}
