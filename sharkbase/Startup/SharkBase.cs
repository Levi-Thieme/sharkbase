using Newtonsoft.Json;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.DataAccess.Index.Repositories;
using SharkBase.DataAccess.Schema.Repositories;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Parsing;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharkBase.Startup
{
    public class SharkBase : IDisposable
    {
        private string workingDirectory;
        private string databaseName;
        private PhysicalStorage store;
        private ITables tables;
        private SchemaRepository schemas;
        private IndexRepository indices;

        public SharkBase()
        {
            workingDirectory = string.Empty;
            databaseName = string.Empty;
        }

        public void Connect(string databaseDirectory)
        {
            this.workingDirectory = databaseDirectory;
            this.databaseName = Path.GetDirectoryName(this.workingDirectory);
            if (!Directory.Exists(databaseDirectory))
            {
                throw new ArgumentException($"The location, {databaseDirectory}, does not exist.");
            }
            var store = new FileStore(workingDirectory);
            this.indices = new FileIndices(store);
            this.schemas = new FileSchemas(store);
            var tableNames = this.schemas.GetAllSchemas().Select(schema => schema.Name);
            var tables = new Tables(store, new IdGenerator(), schemas, indices, tableNames);
            this.tables = tables;
        }

        public void Disconnect() => Dispose();

        private Parser BuildParser()
        {
            var parsers = new List<IParser>();
            parsers.Add(new InsertTableParser());
            parsers.Add(new DeleteTableParser());
            parsers.Add(new InsertRecordParser(this.schemas));
            parsers.Add(new SelectParser(this.schemas));
            parsers.Add(new DeleteRecordParser(this.schemas));
            return new Parser(parsers);
        }

        private CommandBuilder BuildCommandBuilder()
        {
            return new CommandBuilder(this.tables, this.schemas);
        }

        public void Dispose()
        {
            
        }
    }
}
