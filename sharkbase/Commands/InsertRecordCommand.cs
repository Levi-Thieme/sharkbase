﻿using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using System.Linq;

namespace SharkBase.Commands
{
    public class InsertRecordCommand : ICommand
    {
        private InsertRecordStatement statement;
        private TableSchema schema;
        private ITable table;
        private IValueParser valueParser;

        public InsertRecordCommand(InsertRecordStatement statement, TableSchema schema, ITable table, IValueParser valueParser)
        {
            this.statement = statement;
            this.schema = schema;
            this.table = table;
            this.valueParser = valueParser;
        }

        public void Execute()
        {
            var values = valueParser.ParseColumnValues(statement.ColumnValues, schema.Columns.Where(c => !c.HasDefaultValue));
            var recordToInsert = new Record(values);
            table.InsertRecord(recordToInsert);
        }
    }
}
