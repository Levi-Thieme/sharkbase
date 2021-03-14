using SharkBase.DataAccess;
using SharkBase.QueryProcessing.Validation;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.Statements
{
    public class InsertTableStatement : IStatement
    {
        private readonly IStatementValidator validator;
        public string Table { get; set; }
        public IEnumerable<string> Tokens { get; set; }
        public IEnumerable<Column> Columns { get; private set; }

        public InsertTableStatement(IStatementValidator validator, string table, IEnumerable<Column> columns)
        {
            this.validator = validator;
            this.Table = table;
            this.Columns = columns;
        }

        public void Validate() => this.validator.Validate(Table, Tokens);
    }
}
