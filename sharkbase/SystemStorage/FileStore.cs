using System.Linq;
using System.Collections.Generic;
using System.IO;
using SharkBase.DataAccess;

namespace SharkBase.SystemStorage
{
    public class FileStore : PhysicalStorage
    {
        private readonly string workingDirectory;
        private const string TABLE_EXTENSION = ".table";
        private const string SCHEMAS_EXTENSION = ".schema";
        private const string INDEX_EXTENSION = ".index";

        public FileStore(string workingDirectory)
        {
            this.workingDirectory = workingDirectory;
        }

        public void InsertTable(string name) 
        {
            Directory.CreateDirectory(TableDirectoryPath(name));
            File.Create(TableFilePath(name)).Dispose();
        }

        public void DeleteTable(string name)
        {
            if (TableExists(name))
                File.Delete(TableFilePath(name));
        }

        public long Append(string table, MemoryStream stream)
        {
            using (FileStream fstream = new FileStream(TableFilePath(table), FileMode.Append))
            {
                long startPosition = fstream.Position;
                stream.WriteTo(fstream);
                return startPosition;
            }
        }

        public Stream GetTableStream(string table) => new FileStream(TableFilePath(table), FileMode.Open);
        public Stream GetSchemaStream(string tableName) => new FileStream(SchemaFilePath(tableName), FileMode.OpenOrCreate);
        public Stream GetIndexStream(string tableName, string indexName) => new FileStream(IndexFilePath(tableName, indexName), FileMode.OpenOrCreate);

        internal IEnumerable<string> GetTableNames()
        {
            if (!Directory.Exists(workingDirectory))
                Directory.CreateDirectory(workingDirectory);
            return Directory.GetFiles(workingDirectory)
                .Where(file => file.EndsWith(TABLE_EXTENSION))
                .Select(file => Path.GetFileNameWithoutExtension(file));
        }

        public string SchemaFilePath(string tableName) => Path.Combine(workingDirectory, tableName, $"{tableName}{SCHEMAS_EXTENSION}");
        public string IndexFilePath(string tableName, string indexName) => Path.Combine(workingDirectory, tableName, $"{indexName}{INDEX_EXTENSION}");
        public string TableDirectoryPath(string name) => Path.Combine(workingDirectory, name);
        public string TableFilePath(string name) => Path.Combine(workingDirectory, name, $"{name}{TABLE_EXTENSION}");
        private bool TableExists(string name) => File.Exists(TableFilePath(name));

        public void DeleteSchema(string tableName)
        {
            File.Delete(SchemaFilePath(tableName));
        }

        public void DeleteIndex(string tableName, string indexName)
        {
            File.Delete(IndexFilePath(tableName, indexName));
        }

        public void DeleteAllIndices(string tableName)
        {
            var indices = Directory.GetFiles(TableDirectoryPath(tableName));
            foreach (string indexPath in indices)
            {
                File.Delete(indexPath);
            }
        }
    }
}
