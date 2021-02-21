using System;
using System.Collections.Generic;

namespace SharkBase.DataAccess
{
    public interface ITable
    {
        TableSchema Schema { get; }
        void InsertRecord(Record record);
        Record ReadRecord();
        IEnumerable<Record> ReadAllRecords();
        Guid GetUniqueId();
        void DeleteRecords(IEnumerable<Record> guids);
    }
}
