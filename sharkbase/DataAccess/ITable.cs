using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public interface ITable
    {
        TableSchema Schema { get; }
        void InsertRecord(Record record);
        Record ReadRecord();
        IEnumerable<Record> ReadAllRecords();
    }
}
