using SharkBase.DataAccess.Index;
using SharkBase.DataAccess.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.DataAccess.Streaming
{
    public class RecordStream : Streamable<Record>
    {
        private SecondaryIndex<bool> isDeletedIndex;
        private Stream baseStream;
        private BinaryReader reader;
        private Func<BinaryReader, Record> getRecordFromReader;
        private Record current = null;
        public Record Current => this.current;

        public RecordStream(SecondaryIndex<bool> isDeletedIndex, Stream baseStream, Func<BinaryReader, Record> readCallback)
        {
            this.isDeletedIndex = isDeletedIndex;
            this.baseStream = baseStream;
            this.reader = new BinaryReader(this.baseStream);
            this.getRecordFromReader = readCallback;
        }

        private bool endOfStream => this.baseStream.Position >= this.baseStream.Length;

        public bool Read()
        {
            if (endOfStream)
            {
                return false;
            }
            else
            {
                this.current = getNextNonDeletedRecord();
            }
            return this.current != null;
        }

        private Record getNextNonDeletedRecord()
        {
            Record record = null;
            while (record == null && !endOfStream)
            {
                record = getRecordFromReader(this.reader);
                if (this.isDeletedIndex.HasKey(record.GetId()))
                {
                    record = null;
                }
            }
            return record;
        }

        public void Dispose()
        {
            this.baseStream.Dispose();
            this.reader.Dispose();
        }
    }
}
