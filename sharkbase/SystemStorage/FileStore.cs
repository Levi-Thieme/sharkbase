using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace SharkBase.SystemStorage
{
    public class FileStore : PhysicalStorage
    {
        private readonly string workingDirectory;
        private const string TABLE_EXTENSION = ".table";
        private const string SCHEMAS_EXTENSION = ".schema";
        private const string INDEX_EXTENSION = ".index";
        private const string METADATA_EXTENSION = ".metadata";

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

        public Stream GetDatabaseMetadataStream() => new FileStream(DatabaseMetadataPath(), FileMode.OpenOrCreate);
        public Stream GetTableStream(string table) => new FileStream(TableFilePath(table), FileMode.Open);
        public Stream GetSchemaStream(string tableName) => new FileStream(SchemaFilePath(tableName), FileMode.OpenOrCreate);
        public Stream GetOverwritingSchemaStream(string tableName)
        {
            return SchemaExists(tableName) ?
                new FileStream(SchemaFilePath(tableName), FileMode.Truncate) :
                GetSchemaStream(tableName);

        }
        public Stream GetIndexStream(string tableName, string indexName) => new FileStream(IndexFilePath(tableName, indexName), FileMode.OpenOrCreate);
        public Stream GetOverwritingIndexStream(string tableName, string indexName)
        {
            return IndexExists(tableName, indexName) ?
                new FileStream(IndexFilePath(tableName, indexName), FileMode.Truncate) :
                GetIndexStream(tableName, indexName);
        }

        internal IEnumerable<string> GetTableNames()
        {
            if (!Directory.Exists(workingDirectory))
                Directory.CreateDirectory(workingDirectory);
            return Directory.GetFiles(workingDirectory)
                .Where(file => file.EndsWith(TABLE_EXTENSION))
                .Select(file => Path.GetFileNameWithoutExtension(file));
        }

        public string DatabaseMetadataPath()
        {
            string databaseName = new DirectoryInfo(workingDirectory).Name;
            return Path.Combine(workingDirectory, $"{databaseName}{METADATA_EXTENSION}");
        }
        public string SchemaFilePath(string tableName) => Path.Combine(workingDirectory, tableName, $"{tableName}{SCHEMAS_EXTENSION}");
        public bool SchemaExists(string tableName) => File.Exists(SchemaFilePath(tableName));
        public string IndexFilePath(string tableName, string indexName) => Path.Combine(workingDirectory, tableName, $"{indexName}{INDEX_EXTENSION}");
        public bool IndexExists(string tableName, string IndexName) => File.Exists(IndexFilePath(tableName, IndexName));
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
