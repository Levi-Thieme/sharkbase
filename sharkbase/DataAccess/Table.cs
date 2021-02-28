﻿using SharkBase.DataAccess.Index;
using SharkBase.DataAccess.Index.Models;
using SharkBase.Models;
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
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8))
                {
                    string guid = GetUniqueId().ToString();
                    writer.Write(guid);
                    writer.Write(false);
                    record.WriteTo(writer);
                    long recordOffset = store.Append(schema.Name, stream);
                    insertPrimaryIndex(guid, recordOffset);
                    insertIsDeletedIndex(guid);
                }
            }
        }

        private void insertPrimaryIndex(string guid, long offset)
        {
            var primaryIndex = indices.Get(schema.Name);
            primaryIndex.Add(guid, offset);
            indices.Upsert(primaryIndex);
        }

        private void insertIsDeletedIndex(string guid)
        {
            var deletedIndex = indices.GetIsDeletedIndex(schema.Name);
            deletedIndex.Add(guid, false);
            indices.Upsert<bool>(deletedIndex);
        }

        public Record ReadRecord()
        {
            Record defaultDeletedRecord = new Record(new List<Value> { new Value(Guid.NewGuid().ToString()), new Value(true) });
            var record = defaultDeletedRecord;
            using (var stream = store.GetTableStream(schema.Name))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length && record.IsDeleted())
                    {
                        record = readRecordFromStream(reader);
                    } 
                }
            }
            return record.IsDeleted() ? null : record;
        }

        public IEnumerable<Record> ReadAllRecords()
        {
            List<Record> records = new List<Record>();
            using (var stream = store.GetTableStream(schema.Name))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    var streamLength = reader.BaseStream.Length;
                    while (reader.BaseStream.Position < streamLength)
                    {
                        var record = readRecordFromStream(reader);
                        if (!record.IsDeleted())
                            records.Add(record);
                    }
                }
            }
            return records;
        }

        private Record readRecordFromStream(BinaryReader reader)
        {
            var values = new List<object>();
            foreach (var type in schema.Columns.Select(c => c.Type))
            {
                if (ColumnType.Int64 == type)
                    values.Add(reader.ReadInt64());
                else if (ColumnType.String == type)
                    values.Add(reader.ReadString());
                else if (ColumnType.boolean == type)
                    values.Add(reader.ReadBoolean());
            }
            return new Record(values.Select(v => new Value(v)));
        }

        public void DeleteRecords(IEnumerable<Record> records)
        {
            using (var stream = store.GetTableStream(schema.Name))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                {
                    foreach (var record in records)
                    {
                        deleteRecord(record, writer);
                    }
                }
            }
        }

        public void DeleteRecord(Record record)
        {
            record.Delete();
            using (var stream = store.GetTableStream(schema.Name))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: false))
                {
                    deleteRecord(record, writer);
                }
            }
        }

        private void deleteRecord(Record record, BinaryWriter stream)
        {
            const int binaryGuidLength = 37;
            string id = record.GetId();
            long isDeletedOffset = indices.Get(schema.Name).GetValue(id) + binaryGuidLength;
            stream.BaseStream.Seek(isDeletedOffset, SeekOrigin.Begin);
            stream.Write(true);
            indices.GetIsDeletedIndex(schema.Name).Update(id, true);
        }

        public Guid GetUniqueId() => this.idGenerator.GetUniqueId();
    }
}
