using SharkBase.Commands;
using SharkBase.Parsing;
using SharkBase.SystemStorage;
using System;
using System.IO;
using System.Linq;

namespace SharkBase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started Sharkbase");
            var parser = new Parser();
            var commandExecutor = new CommandExecutor(new FileStore(Path.GetTempPath()));
            string input = Console.ReadLine().Trim();
            while (input != "QUIT")
            {
                try
                {
                    var command = parser.Parse(input);
                    Console.WriteLine($"{command.Type} {command.Table} {string.Join(", ", command.Columns.Select(c => c.Name))}");
                    commandExecutor.Execute(command);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                input = Console.ReadLine().Trim();
            }
            Console.WriteLine("Quitting Sharkbase...");
        }
    }
}
