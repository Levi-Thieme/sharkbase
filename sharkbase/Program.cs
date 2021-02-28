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
        static void Main(string[] args)
        {
            Console.WriteLine("Started Sharkbase");
            string input = Console.ReadLine().Trim();
            while (input.ToUpper() != "QUIT")
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}{Environment.NewLine}{e.StackTrace}");
                }
                input = Console.ReadLine().Trim();
            }
            Console.WriteLine("Quitting Sharkbase...");
        }

    }
}
