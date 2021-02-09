using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Statements;

namespace SharkBase.Commands
{
    public class DeleteTableCommand : ICommand
    {
        private DeleteTableStatement statement;
        private readonly ITables tables;

        public DeleteTableCommand(DeleteTableStatement deleteTableStatement, ITables tables)
        {
            this.statement = deleteTableStatement;
            this.tables = tables;
        }

        public void Execute() => tables.Delete(statement.Table);
    }
}
