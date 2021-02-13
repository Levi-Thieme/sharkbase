﻿using System.Linq;
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

        public void Append(string table, MemoryStream stream)
        {
            using (FileStream fstream = new FileStream(TableFilePath(table), FileMode.Append))
            {
                stream.WriteTo(fstream);
            }
        }

        public Stream GetReadStream(string table) => new FileStream(TableFilePath(table), FileMode.Open);

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