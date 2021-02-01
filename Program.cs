using SharkBase.Commands;
using SharkBase.DataAccess;
using SharkBase.Parsing;
using SharkBase.QueryProcessing.Parsing;
using System;
using System.Collections.Generic;

namespace SharkBase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started Sharkbase");
            var parser = BuildParser();
            var commandBuilder = new CommandBuilder();
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
            var tables = new TestTables();
            var parsers = new List<IParser>();
            parsers.Add(new InsertTableParser(tables));
            return new Parser(parsers);
        }

        private class TestTables : ITables
        {
            public List<Table> Tables { get; set; }

            public void Create(string name, IEnumerable<Column> columns)
            {
                Tables.Add(new Table(name, columns));
            }

            public void Delete(string name) => Tables.Remove(Tables.Find(t => t.Name == name));

            public bool Exists(string name) => name == "FOOD" ? true : false;

        }
    }
}
