using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.QueryProcessing.Statements
{
    public class DeleteRecordStatement : IStatement
    {
        private IStatementValidator validator;
        public string Table { get; set; }
        public IEnumerable<string> Tokens { get; set; }

        public DeleteRecordStatement(string table, IEnumerable<string> tokens, IStatementValidator validator)
        {
            this.Table = table;
            this.Tokens = tokens;
            this.validator = validator;
        }

        public void Validate()
        {
            validator.Validate(Table, Tokens);
        }
    }
}
