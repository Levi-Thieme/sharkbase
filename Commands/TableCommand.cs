using SharkBase.DataAccess;
using System.Collections.Generic;

namespace SharkBase.Commands
{
    public class TableCommand : Command
    {
        public IEnumerable<Column> Columns { get; private set; }


        public TableCommand(CommandType type, string table, IEnumerable<Column> columns) : base(type, table)
        {
            this.Columns = columns;
        }

        public TableCommand(CommandType type, string table) : base(type, table)
        {
            this.Columns = new List<Column>();
        }
    }
}
