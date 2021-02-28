using Newtonsoft.Json;
using SharkBase.SystemStorage;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.DataAccess.Index.Repositories
{
    public class FileIndices : IndexRepository
    {
        private const string IS_DELETED = "IS_DELETED";
        private PhysicalStorage store;

        public FileIndices(PhysicalStorage store)
        {
            this.store = store;        
        }

        public void Upsert(PrimaryIndex index)
        {
            using (var stream = store.GetIndexStream(index.Table, index.Name))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8))
                {
                    writer.Write(JsonConvert.SerializeObject(index));
                }
            }
        }

        public void Upsert<K>(SecondaryIndex<K> index)
        {
            using (var stream = store.GetIndexStream(index.Table, index.Name))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8))
                {
                    writer.Write(JsonConvert.SerializeObject(index));
                }
            }
        }

        public PrimaryIndex Get(string table)
        {
            using (var stream = store.GetIndexStream(table, PrimaryIndex.IndexName(table)))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    string json = reader.ReadString();
                    return JsonConvert.DeserializeObject<PrimaryIndex>(json);
                }
            }
        }

        public SecondaryIndex<bool> GetIsDeletedIndex(string table)
        {
            using (var stream = store.GetIndexStream(table, IS_DELETED))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    string json = reader.ReadString();
                    return JsonConvert.DeserializeObject<SecondaryIndex<bool>>(json);
                }
            }
        }

        public SecondaryIndex<K> Get<K>(string table, string indexName)
        {
            using (var stream = store.GetIndexStream(table, indexName))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    string json = reader.ReadString();
                    return JsonConvert.DeserializeObject<SecondaryIndex<K>>(json);
                }
            }
        }

        public void AddPrimaryIndex(string tableName)
        {
            this.Upsert(new PrimaryIndex(tableName, new Dictionary<string, long>())); 
        }

        public void AddSecondaryIndex(string tableName, string indexName)
        {
            this.Upsert<bool>(new SecondaryIndex<bool>(tableName, indexName, new Dictionary<string, bool>()));
        }

        public void RemoveAll(string tableName)
        {
            store.DeleteAllIndices(tableName);
        }
    }
}
