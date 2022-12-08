using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        [JsonIgnore]
        public Player Player { get; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool isRunning { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IInputService input, IOutputService output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            isRunning = true;
            Input.InputReceived += OnInputRecieved;
        }

        public void New()
        {
            Output.WriteLine("Welcome to Zork!");
            Output.WriteLine($"{Player.CurrentRoom}");
            Look();
        }

        public void Load()
        {
            string path = @"player";
            if ((File.Exists(path)))
            {
                if (new FileInfo(path).Length > 0)
                {
                    Restore();
                    Output.WriteLine($"{Player.CurrentRoom}"); 
                    Look();
                }
            }
            else
            {
                New();
            }
        }

        public void OnInputRecieved(object sender, string inputString)
        {
            char seperator = ' ';
            string[] commandTokens = inputString.Split(seperator);

            string verb;
            string subject = null;
            if (commandTokens.Length == 0)
            {
                return;
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

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);
            switch (command)
            {
                case Commands.QUIT:
                    isRunning = false;
                    Output.WriteLine("Thank you for playing!");
                    break;

                case Commands.LOOK:
                    Look();
                    break;

                case Commands.NORTH:
                case Commands.SOUTH:
                case Commands.WEST:
                case Commands.EAST:
                case Commands.UP:
                case Commands.DOWN:
                    Directions direction = (Directions)command;
                    if (verb.Contains("UP"))
                    {
                        Output.WriteLine(Player.Move(direction) && previousRoom.Name == "Up a Tree" ? $"You moved {direction}." : "You cannot climb any higher.");
                    }
                    else
                    {
                        Output.WriteLine(Player.Move(direction) ? $"You moved {direction}." : "The way is shut!");
                    }
                    break;

                case Commands.CLIMB:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("What do you want to climb up?");
                    }
                    else if (subject.Contains("tree"))
                    {
                        Output.WriteLine(Player.Move((Directions)command) ? $"You moved {(Directions)command}." : "You cannot see such thing.");
                    }
                    else if (previousRoom.Name == "Up a Tree")
                    {
                        Output.WriteLine(previousRoom.Name == "Up a Tree" ? $"You moved {(Directions)command}." : "You cannot climb any higher.");
                    }
                    break;

                case Commands.REWARD:
                    Player.Score++;
                    break;

                case Commands.SCORE:
                    Output.WriteLine("Your score would be " + Player.ReturnScore() + ", in " + Player.ReturnMoves() + " move(s).");
                    break;

                case Commands.TAKE:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Take(subject);
                    }
                    break;

                case Commands.DROP:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Drop(subject);
                    }
                    break;

                case Commands.INVENTORY:
                    if (Player.Inventory.Count() == 0)
                    {
                        Output.WriteLine("You are empty handed.");
                    }
                    else
                    {
                        Output.WriteLine("You are carrying:");
                        foreach (Item playerItem in Player.Inventory)
                        {
                            Output.WriteLine(playerItem.Description);
                        }
                    }
                    break;

                case Commands.SAVE:
                    Save();
                    Output.WriteLine("Ok.");
                    break;

                case Commands.RESTORE:
                    Restore();
                    Output.WriteLine("Ok.");
                    break;

                case Commands.HELLO:
                    Output.WriteLine("Good day.");
                    break;

                default:
                    Output.WriteLine("Unknown command.");
                    break;
            }

            if (command != Commands.UNKNOWN)
            {
                Player.Moves++;
            }

            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }

            Output.WriteLine($"\n{Player.CurrentRoom}");
        }

        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.Description);
            }
        }

        private void Take(string itemName)
        {
            Item itemTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemTake == null)
            {
                Output.WriteLine("You can't see any such thing.");
            }
            else
            {
                Player.AddToInventory(itemTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemTake);
                Output.WriteLine("Taken.");
            }
        }

        private void Drop(string itemName)
        {
            Item itemDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemDrop == null)
            {
                Output.WriteLine("You can't see any such thing.");
            }
            else
            {
                Player.CurrentRoom.AddToRoom(itemDrop);
                Player.RemoveFromInventory(itemDrop);
                Output.WriteLine("Dropped.");
            }
        }

        private void Save()
        {
            SaveSystem.SavePlayer(Player, World);
        }

        public void Restore()
        {
            PlayerData data = SaveSystem.LoadPlayer();

            Player.CurrentRoom = data.startingLocation;
            Player.Score = data.score;
            Player.Moves = data.moves;
            foreach (var item in data.inventory)
            {
                Player.AddToInventory(item);
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse<Commands>(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}