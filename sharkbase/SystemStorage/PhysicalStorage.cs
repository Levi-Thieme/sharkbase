using System.IO;

namespace SharkBase.SystemStorage
{
    public interface PhysicalStorage
    {
        void InsertTable(string name);
        void DeleteTable(string name);
        Stream GetDatabaseMetadataStream();
        Stream GetTableStream(string name);
        Stream GetSchemaStream(string tableName);
        Stream GetOverwritingSchemaStream(string tableName);
        Stream GetIndexStream(string tableName, string indexName);
        Stream GetOverwritingIndexStream(string tableName, string IndexName);
        void DeleteSchema(string tableName);
        void DeleteIndex(string tableName, string indexName);
        void DeleteAllIndices(string tableName);
    }
}
