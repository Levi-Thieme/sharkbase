using SharkBase.DataAccess.Index;
using System.Collections.Generic;
using System.IO;

namespace SharkBase.DataAccess
{
    public interface IndexRepository
    {
        void AddPrimaryIndex(string name);
        void AddSecondaryIndex(string tableName, string indexName);
        void RemoveAll(string tableName);
        void Upsert(PrimaryIndex index);
        void Upsert<K>(SecondaryIndex<K> index);
        PrimaryIndex Get(string tableName);
        SecondaryIndex<bool> GetIsDeletedIndex(string tableName);
        SecondaryIndex<K> Get<K>(string tableName, string indexName);
    }
}
