using SharkBase.DataAccess;
using SharkBase.Statements;
using SharkBase.SystemStorage;
using System;
using System.IO;

namespace SharkBase.Commands
{
    public class CommandBuilder
    {
        private ITables tables = new Tables(new FileStore(Path.GetTempPath()));

        public ICommand Build(IStatement statement)
        {
            if (statement is InsertTableStatement)
            {
                return new InsertTableCommand(statement as InsertTableStatement, tables);
            }
            throw new ArgumentException("The provided statement does not correspond to an existing Command.");
        }
    }
}
