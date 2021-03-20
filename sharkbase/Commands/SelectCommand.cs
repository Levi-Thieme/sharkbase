using SharkBase.DataAccess;
using SharkBase.Models;
using SharkBase.Parsing;
using System;
using System.Linq;

namespace SharkBase.Commands
{
    public class SelectCommand : ICommand
    {
        private IStatement statement;
        private ITable table;

        public SelectCommand(IStatement statement, ITable table)
        {
            this.statement = statement;
            this.table = table;
        }

        public void Execute()
        {
            var records = this.table.ReadAllRecords();
            if (statement.Tokens.Any())
            {
                int columnIndex = table.Schema.Columns.ToList().FindIndex(c => c.Name == statement.Tokens.First());
                var type = table.Schema.Columns.ElementAt(columnIndex).Type;
                var value = new ValueParser().ParseValue(statement.Tokens.ElementAt(1), type);
                records = records.Where(record => record.Values.ElementAt(columnIndex).Equals(value));
            }
            if (records.Any())
            {
                foreach (var record in records)
                    Console.WriteLine(record.ToString());
            }
            else
                Console.WriteLine("No results found.");
        }
    }
}
