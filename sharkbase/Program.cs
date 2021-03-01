using System;

namespace SharkBase
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var sharkBase = new Startup.SharkBase())
                {
                    sharkBase.Connect("C:/Users/Levi/AppData/Local/Temp/test_db");
                    string input = Console.ReadLine().Trim();
                    while (input.ToUpper() != "QUIT")
                    {
                        try
                        {
                            var command = sharkBase.Parse(input);
                            command.Execute();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"{e.Message}");
                        }
                        input = Console.ReadLine().Trim();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }
            Console.WriteLine("Quitting Sharkbase...");
        }
    }

}

