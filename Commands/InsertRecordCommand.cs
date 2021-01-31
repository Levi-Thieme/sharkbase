using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.Commands
{
    public class InsertRecordCommand : Command
    {
        public IEnumerable<ColumnValue> ColumnValues { get; private set; }

        public InsertRecordCommand(CommandType type, string table) : base(type, table)
        {
            this.ColumnValues = new List<ColumnValue>();
        }

        public InsertRecordCommand(CommandType type, string table, IEnumerable<ColumnValue> values) : base(type, table)
        {
            this.ColumnValues = values;
        }
    }

    public class ColumnValue
    {
        public string Column { get; private set; }
        public object Value { get; private set; }

        public ColumnValue(string column, string value)
        {
            this.Column = column;
            this.Value = value;
        }
    }
}
