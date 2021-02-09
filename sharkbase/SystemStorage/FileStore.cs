using System.Linq;
using System.Collections.Generic;
using System.IO;
using SharkBase.DataAccess;

namespace SharkBase.SystemStorage
{
    public class FileStore : ISystemStore
    {
        private readonly string workingDirectory;
        private const string TABLE_EXTENSION = ".table";

        public FileStore(string workingDirectory)
        {
            this.workingDirectory = workingDirectory;
        }

        public void InsertTable(string name) 
        {
            File.Create(TableFilePath(name)).Dispose();
        }

        public void DeleteTable(string name)
        {
            if (TableExists(name))
                File.Delete(TableFilePath(name));
        }

        public void InsertRecord(string name, IEnumerable<object> values)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(TableFilePath(name), FileMode.Append)))
            {
                foreach (object value in values)
                {
                    if (value is long)
                        writer.Write((long)value);
                    else if (value is string)
                        writer.Write((string)value);
                }
                    
            }
        }

        public void WriteFromPosition(string table, MemoryStream stream, long position)
        {
            using (FileStream fstream = new FileStream(TableFilePath(table), FileMode.OpenOrCreate))
            {
                fstream.Seek(position, SeekOrigin.Begin);
                stream.WriteTo(fstream);
            }
        }

        public IEnumerable<object> ReadRecord(string name, TableSchema schema)
        {
            var values = new List<object>();
            using (BinaryReader reader = new BinaryReader(new FileStream(TableFilePath(name), FileMode.Open)))
            {
                foreach (var column in schema.Columns)
                {
                    if (column.Type == ColumnType.Int64)
                        values.Add(reader.ReadInt64());
                    else if (column.Type == ColumnType.Char128)
                        values.Add(reader.ReadString());
                }
            }
            return values;
        }

        internal IEnumerable<string> GetTableNames()
        {
            return Directory.GetFiles(workingDirectory)
                .Where(file => file.EndsWith(TABLE_EXTENSION))
                .Select(file => Path.GetFileNameWithoutExtension(file));
        }

        private string TableNameWithExtension(string name) => $"{name}{TABLE_EXTENSION}";
        private string TableFilePath(string name) => Path.Combine(workingDirectory, TableNameWithExtension(name));
        private bool TableExists(string name) => File.Exists(TableFilePath(name));

    }
}
