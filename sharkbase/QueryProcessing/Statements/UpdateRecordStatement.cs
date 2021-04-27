using SharkBase.Models.Values;
using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.QueryProcessing.Statements
{
    public class UpdateRecordStatement : IStatement
    {
        public string Column { get; private set; }
        public string Value { get; private set; }
        public string WhereColumn { get; private set; }
        public string WhereColumnValue { get; private set; }
        public string Table { get; set; }
        public IEnumerable<string> Tokens { get; set; }

        public UpdateRecordStatement(string table, string column, string value)
        {
            Table = table;
            Column = column;
            Value = value;
            Tokens = new List<string>();
        }

        public UpdateRecordStatement(string table, string column, string value, string whereColumn, string whereColumnValue)
        {
            Table = table;
            Column = column;
            Value = value;
            WhereColumn = whereColumn;
            WhereColumnValue = whereColumnValue;
            Tokens = new List<string>();
        }

        public void Validate() { }
    }
}
