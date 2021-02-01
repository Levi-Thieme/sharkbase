using SharkBase.DataAccess;
using SharkBase.Statements;

namespace SharkBase.Commands
{
    public class InsertTableCommand : ICommand
    {
        private InsertTableStatement statement;
        private ITables tables;

        public InsertTableCommand(InsertTableStatement insertTableStatement, ITables tables)
        {
            this.statement = insertTableStatement;
            this.tables = tables;
        }

        public void Execute()
        {
            statement.ParseColumnDefinitions();
            tables.Create(statement.Table, statement.Columns);
        }
    }
}
