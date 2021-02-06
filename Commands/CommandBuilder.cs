using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Statements;
using SharkBase.Statements;
using SharkBase.SystemStorage;
using System;

namespace SharkBase.Commands
{
    public class CommandBuilder
    {
        private ITables tables;

        public CommandBuilder(ITables tables)
        {
            this.tables = tables;
        }

        public ICommand Build(IStatement statement)
        {
            if (statement is InsertTableStatement)
            {
                return new InsertTableCommand(statement as InsertTableStatement, tables);
            }
            else if (statement is DeleteTableStatement)
            {
                return new DeleteTableCommand(statement as DeleteTableStatement, tables);
            }
            throw new ArgumentException("The provided statement does not correspond to an existing Command.");
        }
    }
}
