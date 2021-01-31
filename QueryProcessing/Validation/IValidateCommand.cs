using SharkBase.Commands;
using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.QueryProcessing.Validation
{
    public interface IValidateInsertRecordCommand
    {
        void Validate(InsertRecordCommand command, Table table);
    }
}
