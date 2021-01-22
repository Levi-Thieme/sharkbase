using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.Commands
{
    public class Command
    {
        public CommandType Type { get; set; }
        public string Table { get; set; }
        public Dictionary<string, ColumnType> Columns { get; set; }


        public Command(CommandType type, string table)
        {
            this.Type = type;
            this.Table = table;
            this.Columns = new Dictionary<string, ColumnType>();
        }
    }
}
