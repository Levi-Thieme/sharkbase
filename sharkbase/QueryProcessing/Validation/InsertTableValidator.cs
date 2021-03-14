using SharkBase.DataAccess;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SharkBase.QueryProcessing.Validation
{
    public class InsertTableValidator : IStatementValidator
    {
        public void Validate(string table, IEnumerable<string> tokens) { }
    }
}
