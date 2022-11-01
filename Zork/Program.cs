using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            const string defaultGameFilename = "Content/Zork.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultGameFilename);
            Game game = Game.Load(gameFilename);

            var output = new ConsoleOutputService();

            Console.WriteLine("Welcome to Zork!");
            game.Run(output);
        }

        private enum CommandLineArguments
        {
            GameFilename = 0
        }    
    }
}