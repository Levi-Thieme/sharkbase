using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;

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

        public void InsertTable(string name, IEnumerable<Column> columns)
        {
            File.Create(TableFilePath(name)).Dispose();
        }

        public void DeleteTable(string name)
        {
            if (TableExists(name))
                File.Delete(TableFilePath(name));
        }

        private string TableNameWithExtension(string name) => $"{name}{TABLE_EXTENSION}";
        private string TableFilePath(string name) => Path.Combine(workingDirectory, TableNameWithExtension(name));
        private bool TableExists(string name) => File.Exists(TableFilePath(name));
    }
}
