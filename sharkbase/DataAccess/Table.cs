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
        private TableSchema schema;
        private string name;
        private long position = 0;

        public Table(ISystemStore store, string name)
        {
            this.store = store;
            this.name = name;
            position = store.TableBytes(schema.Name);
        }

        public Table(ISystemStore store, TableSchema schema)
        {
            this.store = store;
            this.schema = schema;
            position = store.TableBytes(schema.Name);
        }

        public void InsertRecord(Record record)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8))
                {
                    writeRecord(writer, record);
                    store.Write(schema.Name, stream, position);
                    position += getRecordByteCount();
                }
            }
        }

        private void writeRecord(BinaryWriter writer, Record record)
        {
            for (int i = 0; i < schema.Columns.Count(); i++)
            {
                var column = schema.Columns.ElementAt(i);
                var value = record.Values.ElementAt(i);
                if (ColumnType.Int64 == column.Type)
                    writer.Write((long)value);
                else if (ColumnType.Char128 == column.Type)
                    writer.Write((string)value);
            }
        }

        public IEnumerable<Record> ReadFirstNRecord(int n)
        {
            int byteCount = getRecordByteCount();
            int totalBytesToRead = byteCount * n;
            byte[] buffer = new byte[totalBytesToRead];

            store.Read(schema.Name, buffer, 0, totalBytesToRead);

            var records = new List<Record>();
            using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
            {
                for (int i = 0; i < n; i++)
                {
                    records.Add(readBinaryRecord(reader));
                }
            }
            return records;
        }

        public IEnumerable<Record> ReadAllRecords()
        {
            return ReadFirstNRecord((int)store.TableBytes(schema.Name) / getRecordByteCount());
        }

        private Record readBinaryRecord(BinaryReader reader)
        {
            var values = new List<object>();
            foreach (var column in schema.Columns)
            {
                if (column.Type == ColumnType.Int64)
                    values.Add(reader.ReadInt64());
                else if (column.Type == ColumnType.Char128)
                    values.Add(reader.ReadString());
            }
            return new Record(values);
        }

        public long RecordCount()
        {
            long bytes = store.TableBytes(schema.Name);
            return bytes / getRecordByteCount();
        }

        private int getRecordByteCount()
        {
            return schema.Columns.Select(c => byteCounts[c.Type]).Sum();
        }

        private Dictionary<ColumnType, int> byteCounts = new Dictionary<ColumnType, int>()
        {
            { ColumnType.Int64, 8 },
            { ColumnType.Char128, 128 }
        };
    }
}
