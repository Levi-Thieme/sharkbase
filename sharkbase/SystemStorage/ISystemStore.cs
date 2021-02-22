using System.IO;

namespace SharkBase.SystemStorage
{
    public interface ISystemStore
    {
        void InsertTable(string name);
        void DeleteTable(string name);
        long Append(string name, MemoryStream data);
        Stream GetStream(string name);
    }
}
