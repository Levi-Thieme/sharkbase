using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.Commands
{
    public class Command
    {
        public CommandType Type { get; private set; }
        public string Table { get; private set; }

        public Command(CommandType type, string table)
        {
            this.Type = type;
            this.Table = table;
        }
    }
}
