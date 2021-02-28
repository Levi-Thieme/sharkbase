using System.IO;

namespace SharkBase.SystemStorage
{
    public interface PhysicalStorage
    {
        void InsertTable(string name);
        void DeleteTable(string name);
        long Append(string name, MemoryStream data);
        Stream GetTableStream(string name);
        Stream GetSchemaStream(string databaseName);
        Stream GetIndexStream(string indexName);
    }
}
