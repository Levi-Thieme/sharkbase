using System.IO;

namespace SharkBase.SystemStorage
{
    public interface ISystemStore
    {
        void InsertTable(string name);
        void DeleteTable(string name);
        void Append(string name, MemoryStream data);
        Stream GetReadStream(string name);
    }
}
