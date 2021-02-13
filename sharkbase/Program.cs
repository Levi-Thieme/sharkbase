using Newtonsoft.Json;
using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Parsing;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase
{
    class Program
    {
        private static readonly string workingDirectory = Path.GetTempPath() + "/test_db";
        private ITables tables;
        private ISchemaProvider schemas;

        static void Main(string[] args)
        {
            var program = new Program();
            program.Initialize();
            var parser = program.BuildParser();
            var commandBuilder = program.BuildCommandBuilder();

            Console.WriteLine("Started Sharkbase");
            string input = Console.ReadLine().Trim();
            while (input != "QUIT")
            {
                try
                {
                    IStatement statement = parser.Parse(input);
                    statement.Validate();
                    ICommand command = commandBuilder.Build(statement);
                    command.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                input = Console.ReadLine().Trim();
            }
            program.WriteTableSchemas(Path.Join(workingDirectory, $"test_db.schemas") , program.schemas.GetAllSchemas());
            Console.WriteLine("Quitting Sharkbase...");
        }

        private void Initialize()
        {
            var store = new FileStore(workingDirectory);
            var tableSchemas = GetTableSchema(store.SchemaFilePath("test_db"));
            var tableNames = store.GetTableNames();
            var tables = new Tables(new FileStore(workingDirectory), tableNames, tableSchemas);
            this.tables = tables;
            this.schemas = tables;
        }

        private IEnumerable<TableSchema> GetTableSchema(string filepath)
        {
            if (!File.Exists(filepath))
                return new List<TableSchema>();
            string text = File.ReadAllText(filepath);
            if (string.IsNullOrEmpty(text))
                return new List<TableSchema>();
            return JsonConvert.DeserializeObject<IEnumerable<TableSchema>>(text);
        }

        private void WriteTableSchemas(string filepath, IEnumerable<TableSchema> schemas)
        {
            File.WriteAllText(filepath, JsonConvert.SerializeObject(schemas), Encoding.UTF8);
        }

        private Parser BuildParser()
        {
            var parsers = new List<IParser>();
            parsers.Add(new InsertTableParser());
            parsers.Add(new DeleteTableParser());
            parsers.Add(new InsertRecordParser(this.schemas));
            return new Parser(parsers);
        }

        private CommandBuilder BuildCommandBuilder()
        {
            return new CommandBuilder(this.tables, this.schemas);
        }
    }
}
