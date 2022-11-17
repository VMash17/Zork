using System;
using System.IO;
using Newtonsoft.Json;
using Zork.Common;

namespace Zork.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            const string defaultGameFilename = @"Content/Zork.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultGameFilename);
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFilename));

            var input = new ConsoleInputServices();
            var output = new ConsoleOutputService();
            game.Run(input, output);

            game.Player.MovesChanged += Player_MovesChanged;

            while (game.isRunning)
            {
                game.Output.Write("> ");
                input.ProcessInput();
            }

            output.WriteLine("Thank you for playing!");
        }

        private static void Player_MovesChanged(object sender, int moves)
        {
            Console.WriteLine($"You've made {moves} moves.");
        }

        private enum CommandLineArguments
        {
            GameFilename = 0
        }
    }
}