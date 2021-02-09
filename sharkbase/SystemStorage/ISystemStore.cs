using System.Collections.Generic;

namespace SharkBase.SystemStorage
{
    public interface ISystemStore
    {
        void InsertTable(string name);
        void DeleteTable(string name);
        void InsertRecord(string name, IEnumerable<object> values);
    }
}
