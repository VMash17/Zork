using System;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    public class Game
    {
        public World World { get; private set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        public IOutputService Output { get; private set; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;
        }

        public void Run(IOutputService output)
        {
            Output = output;

            Room previousRoom = null;
            Item item = null;
            bool isRunning = true;
            while (isRunning)
            {
                Output.WriteLine(Player.Location);

                if (previousRoom != Player.Location)
                {
                    Output.WriteLine(Player.Location.Description);
                    previousRoom = Player.Location;

                    foreach (Item roomItem in Player.Location.Inventory)
                    {
                        Output.WriteLine(roomItem.Description);
                    }
                    Output.Write('\n');
                }
                Output.Write("> ");

                string inputString = Console.ReadLine().Trim();
                char seperator = ' ';
                string[] commandTokens = inputString.Split(seperator);

                string verb = null;
                string subject = null;
                if (commandTokens.Length == 0)
                {
                    continue;
                }
                else if (commandTokens.Length == 1)
                {
                    verb = commandTokens[0];
                }
                else
                {
                    verb = commandTokens[0];
                    subject = commandTokens[1];
                }

                Commands command = ToCommand(verb);
                string outputString = null;
                switch (command)
                {
                    case Commands.QUIT:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.LOOK:
                        outputString = Player.Location.Description + '\n';

                        foreach (Item roomItem in Player.Location.Inventory)
                        {
                            outputString += roomItem.Description + '\n';
                        }
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.WEST:
                    case Commands.EAST:
                        Directions direction = Enum.Parse<Directions>(command.ToString(), true);
                        if (Player.Move(direction))
                        {
                            outputString = $"You moved {direction}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    case Commands.REWARD:
                        Player.IncreaseScore();
                        break;

                    case Commands.SCORE:
                        outputString = "Your score would be " + Player.ReturnScore() + ", in " + Player.ReturnMoves() + " move(s).";
                        break;

                    case Commands.TAKE:
                        item = Player.Location.Take(subject);
                        if (subject == null)
                        {
                            outputString = "This command requires a subject.\n";
                        }
                        else if (item != null)
                        {
                            Player.AddToInventory(item);
                            outputString = "Taken.\n";
                        }    
                        else
                        {
                            outputString = "You can't see any such thing.\n";
                        }
                        break;

                    case Commands.DROP:
                        item = Player.RemoveFromInventory(subject);
                        if (subject == null)
                        {
                            outputString = "This command requires a subject.\n";
                        }
                        else if (item != null)
                        {
                            Player.Location.AddToRoom(item);
                            outputString = "Dropped.\n";
                        }
                        else
                        {
                            outputString = "You can't see any such thing.\n";
                        }
                        break;

                    case Commands.INVENTORY:
                        if (Player.Inventory.Count == 0)
                        {
                            outputString = "You are empty handed.\n";
                        }
                        else
                        {
                            foreach(Item playerItem in Player.Inventory)
                            {
                                Output.WriteLine(playerItem.Description);
                            }
                        }
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }
                Output.WriteLine(outputString);
            }
        }

        public static Game Load(string filename)
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
            game.Player = game.World.SpawnPlayer();

            return game;
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}