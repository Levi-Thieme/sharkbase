using System.IO;

namespace SharkBase.SystemStorage
{
    public interface PhysicalStorage
    {
        void InsertTable(string name);
        void DeleteTable(string name);
        long Append(string name, MemoryStream data);
        Stream GetTableStream(string name);
        Stream GetSchemaStream(string tableName);
        Stream GetIndexStream(string tableName, string indexName);
        void DeleteSchema(string tableName);
        void DeleteIndex(string tableName, string indexName);
        void DeleteAllIndices(string tableName);
    }
}
