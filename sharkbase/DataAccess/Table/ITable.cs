using SharkBase.DataAccess.Streaming;
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
        Streamable<Record> ReadAll();
        Guid GetUniqueId();
        void DeleteRecords(IEnumerable<Record> records);
        void DeleteRecord(Record record);
    }
}
