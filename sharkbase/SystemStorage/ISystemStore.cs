using System.Collections.Generic;
using System.IO;

namespace SharkBase.SystemStorage
{
    public interface ISystemStore
    {
        void InsertTable(string name);
        void DeleteTable(string name);
        void InsertRecord(string name, IEnumerable<object> values);
        void Write(string name, MemoryStream data, long offset);
        public void Read(string table, byte[] buffer, long position, int count);
        public long TableBytes(string table);
    }
}
