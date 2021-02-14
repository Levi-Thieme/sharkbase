using SharkBase.DataAccess;
using System;

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
            foreach (var record in records)
            {
                Console.WriteLine(record.ToString());
            }
        }
    }
}
