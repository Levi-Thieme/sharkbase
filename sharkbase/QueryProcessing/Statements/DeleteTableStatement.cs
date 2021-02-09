using SharkBase.QueryProcessing.Validation;
using System;
using System.Collections.Generic;

namespace SharkBase.QueryProcessing.Statements
{
    public class DeleteTableStatement : IStatement
    {
        public string Table { get; set; }
        private readonly IStatementValidator validator;
        public IEnumerable<string> Tokens { get; set; }

        public DeleteTableStatement(string table, IStatementValidator validator)
        {
            this.Table = table;
            this.validator = validator;
        }

        public void Validate()
        {
            validator.Validate(Table, Tokens);
        }
    }
}
