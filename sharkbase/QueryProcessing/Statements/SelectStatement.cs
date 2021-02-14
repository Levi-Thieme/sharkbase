using SharkBase.QueryProcessing.Validation;
using System.Collections.Generic;

namespace SharkBase.QueryProcessing.Statements
{
    public class SelectStatement : IStatement
    {
        public string Table { get; set; }
        public IEnumerable<string> Tokens { get; set; }
        private IStatementValidator validator;

        public SelectStatement(IStatementValidator validator, string table, IEnumerable<string> tokens)
        {
            this.validator = validator;
            this.Table = table;
            this.Tokens = tokens;
        }

        public void Validate() => validator.Validate(Table, Tokens);
    }
}
