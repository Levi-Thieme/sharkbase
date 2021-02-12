using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public interface ITable
    {
        void InsertRecord(Record record);
    }
}
