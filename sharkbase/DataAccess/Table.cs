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
        private ISystemStore store;
        private readonly TableSchema schema;
        private readonly Index index;
        private IGenerateId idGenerator;
        public TableSchema Schema => this.schema;

        public Table(ISystemStore store, TableSchema schema, DataAccess.Index index, IGenerateId idGenerator)
        {
            this.store = store;
            this.schema = schema;
            this.index = index;
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
                    record.WriteTo(writer);
                    long recordOffset = store.Append(schema.Name, stream);
                    index.Add(guid, recordOffset);
                }
            }
        }

        public Record ReadRecord()
        {
            using (var stream = store.GetReadStream(schema.Name))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    return readRecordFromStream(reader);
                }
            }
        }

        public IEnumerable<Record> ReadAllRecords()
        {
            List<Record> records = new List<Record>();
            using (var stream = store.GetReadStream(schema.Name))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8))
                {
                    var streamLength = reader.BaseStream.Length;
                    while (reader.BaseStream.Position < streamLength)
                    {
                        records.Add(readRecordFromStream(reader));
                    }
                }
            }
            return records;
        }

        private Record readRecordFromStream(BinaryReader reader)
        {
            var values = new List<object>();
            values.Add(reader.ReadString());
            foreach (var type in schema.Columns.Select(c => c.Type))
            {
                if (ColumnType.Int64 == type)
                    values.Add(reader.ReadInt64());
                else if (ColumnType.String == type)
                    values.Add(reader.ReadString());
            }
            return new Record(values.Select(v => new Value(v)));
        }

        public Guid GetUniqueId() => this.idGenerator.GetUniqueId();
    }
}
