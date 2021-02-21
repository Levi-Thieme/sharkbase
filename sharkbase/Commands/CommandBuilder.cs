using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Statements;
using SharkBase.Statements;
using System;

namespace SharkBase.Commands
{
    public class CommandBuilder
    {
        private ITables tables;
        private ISchemaProvider schemas;

        public CommandBuilder(ITables tables, ISchemaProvider schemas)
        {
            this.tables = tables;
            this.schemas = schemas;
        }

        public ICommand Build(IStatement statement)
        {
            if (statement is InsertTableStatement)
            {
                return new InsertTableCommand(statement as InsertTableStatement, tables);
            }
            else if (statement is DeleteTableStatement)
            {
                return new DeleteTableCommand(statement as DeleteTableStatement, tables);
            }
            else if (statement is InsertRecordStatement)
            {
                return new InsertRecordCommand(
                    statement as InsertRecordStatement,
                    schemas.GetSchema(statement.Table),
                    tables.GetByName(statement.Table),
                    new ValueParser()
                );
            }
            else if (statement is SelectStatement selectStatement)
            {
                return new SelectCommand(selectStatement, tables.GetByName(selectStatement.Table));
            }
            else if (statement is DeleteRecordStatement deleteRecordStatement)
            {
                return new DeleteRecordCommand(deleteRecordStatement, tables.GetByName(deleteRecordStatement.Table));
            }
            throw new ArgumentException("The provided statement does not correspond to an existing Command.");
        }
    }
}
