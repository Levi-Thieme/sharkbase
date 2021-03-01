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
        private PhysicalStorage store;
        private ITables tables;
        private SchemaRepository schemas;
        private IndexRepository indices;
        private DatabaseMetadata metadata;
        private Parser parser;
        private CommandBuilder commandBuilder;

        public SharkBase()
        {
            workingDirectory = string.Empty;
        }

        public void Connect(string databaseDirectory)
        {
            this.workingDirectory = databaseDirectory;
            if (!Directory.Exists(databaseDirectory))
            {
                throw new ArgumentException($"The location, {databaseDirectory}, does not exist.");
            }
            this.store = new FileStore(workingDirectory);
            this.metadata = GetMetadata();
            this.indices = new FileIndices(store);
            this.schemas = new FileSchemas(store);
            this.tables = new Tables(store, new IdGenerator(), schemas, indices, metadata.TableNames);
            this.parser = BuildParser();
            this.commandBuilder = BuildCommandBuilder();
        }

        public void Disconnect() => Dispose();

        public ICommand Parse(string input)
        {
            if (parser.IsParsable(input)) 
            {
                var statement = parser.Parse(input);
                return this.commandBuilder.Build(statement); 
            }
            else
            {
                throw new ArgumentException($"Unable to parser: {input}");
            }
        }

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

        private DatabaseMetadata GetMetadata()
        {
            using (var stream = store.GetDatabaseMetadataStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    string metadataJson = reader.BaseStream.Length > 0 ? reader.ReadString() : string.Empty;
                    return string.IsNullOrEmpty(metadataJson) ? new DatabaseMetadata() : JsonConvert.DeserializeObject<DatabaseMetadata>(metadataJson);
                }
            }
        }

        private void SaveMetadata()
        {
            using (var stream = store.GetDatabaseMetadataStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(metadata));
                }
            }
        }

        public void Dispose()
        {
            this.metadata = new DatabaseMetadata(tables.TableNames());
            SaveMetadata();
        }
    }
}
