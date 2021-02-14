using System.Collections.Generic;

namespace SharkBase.QueryProcessing.Validation
{
    public class AlwaysValidValidator : IStatementValidator
    {
        public void Validate(string table, IEnumerable<string> tokens) { }
    }
}
