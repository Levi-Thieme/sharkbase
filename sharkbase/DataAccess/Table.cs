using SharkBase.DataAccess.Index;
using SharkBase.DataAccess.Index.Models;
using SharkBase.Models;
using SharkBase.Models.Values;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharkBase.DataAccess
{
    public class Table : ITable
    {
        private PhysicalStorage store;
        private readonly TableSchema schema;
        private readonly IndexRepository indices;
        private IGenerateId idGenerator;
        public TableSchema Schema => this.schema;

        public Table(PhysicalStorage store, TableSchema schema, IndexRepository indices, IGenerateId idGenerator)
        {
            this.store = store;
            this.schema = schema;
            this.indices = indices;
            this.idGenerator = idGenerator;
        }

        public void InsertRecord(Record record)
        {
            var primaryIndex = indices.Get(schema.Name);
            var isDeletedIndex = indices.GetIsDeletedIndex(schema.Name);
            using (var stream = store.GetTableStream(schema.Name))
            {
                stream.Seek(0, SeekOrigin.End);
                using (var writer = new BinaryWriter(stream, Encoding.UTF8))
                {
                    string guid = GetUniqueId().ToString();
                    string replacedRecordId = string.Empty;
                    long recordOffset = stream.Length;
                    var deletedRecords = isDeletedIndex.Indices.Where(pair => pair.Value);
                    if (deletedRecords.Any())
                    {
                        replacedRecordId = deletedRecords.First().Key;
                        recordOffset = primaryIndex.GetValue(replacedRecordId);
                    }
                    stream.Seek(recordOffset, SeekOrigin.Begin);
                    writer.Write(guid);
                    record.Write(writer);
                    primaryIndex.Add(guid, recordOffset);
                    if (replacedRecordId != string.Empty)
                    {
                        isDeletedIndex.Remove(replacedRecordId);
                        primaryIndex.Remove(replacedRecordId);
                    }
                    indices.Upsert(primaryIndex);
                    indices.Upsert(isDeletedIndex);
                }
            }
        }

        public Record ReadRecord()
        {
            Record record = null;
            using (var stream = store.GetTableStream(schema.Name))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    if (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        record = readRecordFromStream(reader);
                    } 
                }
            }
            return record;
        }

        public IEnumerable<Record> ReadAllRecords()
        {
            var isDeletedIndex = indices.GetIsDeletedIndex(schema.Name);
            List<Record> records = new List<Record>();
            using (var stream = store.GetTableStream(schema.Name))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    var streamLength = reader.BaseStream.Length;
                    while (reader.BaseStream.Position < streamLength)
                    {
                        var record = readRecordFromStream(reader);
                        if (!isDeletedIndex.Indices.ContainsKey(record.GetId()))
                            records.Add(record);
                    }
                }
            }
            return records;
        }

        private Record readRecordFromStream(BinaryReader reader)
        {
            var values = new List<Value>();
            foreach (var type in schema.Columns.Select(c => c.Type))
            {
                Value value = null;
                if (DataTypes.Int64 == type)
                {
                    value = new LongValue();
                    value.Read(reader);
                }
                else if (DataTypes.String == type)
                {
                    value = new StringValue();
                    value.Read(reader);
                }
                else if (DataTypes.boolean == type)
                {
                    value = new BoolValue();
                    value.Read(reader);
                }
                if (value != null)
                {
                    values.Add(value);
                }
            }
            return new Record(values);
        }

        public void DeleteRecords(IEnumerable<Record> records)
        {
            var isDeletedIndex = indices.GetIsDeletedIndex(schema.Name);
            foreach (var record in records)
                isDeletedIndex.Add(record.GetId(), true);
            indices.Upsert(isDeletedIndex);
        }

        public void DeleteRecord(Record record)
        {
            var isDeletedIndex = indices.GetIsDeletedIndex(schema.Name);
            isDeletedIndex.Add(record.GetId(), true);
            indices.Upsert(isDeletedIndex);
        }

        public Guid GetUniqueId() => this.idGenerator.GetUniqueId();
    }
}
