using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ISystemStore store;

        public CommandExecutor(ISystemStore store)
        {
            this.store = store;
        }

        public void Execute(TableCommand command)
        {
            if (command.Type == CommandType.Delete)
                this.store.DeleteTable(command.Table);
            else if (command.Type == CommandType.Insert)
                this.store.InsertTable(command.Table, command.Columns);
        }
    }
}
