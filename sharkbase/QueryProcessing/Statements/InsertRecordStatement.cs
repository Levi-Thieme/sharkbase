using SharkBase.QueryProcessing.Validation;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.QueryProcessing.Statements
{
    public class InsertRecordStatement : IStatement
    {
        private IStatementValidator validator;
        public string Table { get; set; }
        public IEnumerable<string> Tokens { get; set; }
        public IEnumerable<string> ColumnValues { get; private set; }
        

        public InsertRecordStatement(string table, IEnumerable<string> tokens, IStatementValidator validator)
        {
            this.validator = validator;
            this.Table = table;
            this.Tokens = tokens;
            this.ColumnValues = tokens;
        }

        public void Validate()
        {
            validator.Validate(Table, Tokens);
        }
    }
}
