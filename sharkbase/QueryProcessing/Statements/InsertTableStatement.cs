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

        public InsertTableStatement(IStatementValidator validator)
        {
            this.validator = validator;
            this.Columns = new List<Column>();
        }

        public void Validate() => this.validator.Validate(Table, Tokens);

        public void ParseColumnDefinitions()
        {
            var columnDefinitions = new List<Column>();
            for (int i = 0; i < Tokens.Count(); i += 2)
            {
                columnDefinitions.Add(new Column(ColumnTypes.ColumnTypeByName[Tokens.ElementAt(i)], Tokens.ElementAt(i + 1)));
            }
            this.Columns = columnDefinitions;
        }
    }
}
