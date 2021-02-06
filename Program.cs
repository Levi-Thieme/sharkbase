using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Parsing;
using SharkBase.SystemStorage;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharkBase
{
    class Program
    {
        private static readonly string workingDirectory = Path.GetTempPath() + "/test_db";

        static void Main(string[] args)
        {
            Console.WriteLine("Started Sharkbase");
            var parser = BuildParser();
            var commandBuilder = BuildCommandBuilder();
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
            Console.WriteLine("Quitting Sharkbase...");
        }

        private static Parser BuildParser()
        {
            var parsers = new List<IParser>();
            parsers.Add(new InsertTableParser());
            parsers.Add(new DeleteTableParser());
            return new Parser(parsers);
        }

        private static CommandBuilder BuildCommandBuilder()
        {
            var filestore = new FileStore(workingDirectory);
            var tables = new Tables(filestore, filestore.GetTableNames());
            return new CommandBuilder(tables);
        }
    }
}
