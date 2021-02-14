using SharkBase.QueryProcessing.Validation;
using System.Collections.Generic;

namespace SharkBase.QueryProcessing.Statements
{
    public class SelectStatement : IStatement
    {
        public string Table { get; set; }
        public IEnumerable<string> Tokens { get; set; }
        private IStatementValidator validator;

        public SelectStatement(IStatementValidator validator)
        {
            this.validator = validator;
        }

        public void Validate() => validator.Validate(Table, Tokens);
    }
}
