using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.QueryProcessing.Validation
{
    public interface IStatementValidator
    {
        void Validate(string table, IEnumerable<string> tokens);
    }
}
