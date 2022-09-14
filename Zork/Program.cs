using System;

namespace Zork
{
    class Program
    {
        private static Room CurrentRoom
        {
            get 
            { 
                return rooms[Location.Row, Location.Column];
                   
            }
        }

        static void Main(string[] args)
        {
            InitializeRoomDescriptions();

            Console.WriteLine("Welcome to Zork!");

            bool isRunning = true;
            
            while (isRunning == true)
            {
                Console.Write($"{CurrentRoom}\n> ");
                string inputString = Console.ReadLine().Trim();

                Commands command = ToCommand(inputString);

                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing.";
                        isRunning = false;
                        break;
                    case Commands.LOOK:
                        outputString = CurrentRoom.Description;
                        break;
                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.WEST:
                    case Commands.EAST:
                        if (Move(command))
                        {
                            outputString = $"You moved {command}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;
                    default:
                        outputString = "Unknown command.";
                        break;
                }
                Console.WriteLine(outputString);
            }  
        }

        private static Commands ToCommand(string commandString)
        {
            if (Enum.TryParse<Commands>(commandString, true, out Commands command))
            {
                return command;
            }
            else
            {
                return Commands.UNKNOWN;
            }

        }
        private static void InitializeRoomDescriptions()
        {
            rooms[0, 0].Description = "You are on a rocky-strewn trail.";
            rooms[0, 1].Description = "You are facing the south side of a white house. There is no door here, and all the windows are barred.";
            rooms[0, 2].Description = "You are the top of the Great Canyon on its south wall.";
            
            rooms[1, 0].Description = "This is a forest, with trees in all directions around you.";
            rooms[1, 1].Description = "This is a open field west of a white house, with a boarded front door.";
            rooms[1, 2].Description = "You are behind the white house. In one corner at the house there is a small window which is slightly ajar.";

            rooms[2, 0].Description = "This is a dimly lit forest, with large trees all around, To the east, there appears to be sunlight.";
            rooms[2, 1].Description = "You are facing the north side of a white house. There is no door here, and all the windows are barred.";
            rooms[2, 2].Description = "You are in a clearing, with a forest surrounding you on the west and south.";
        }

        private static readonly Room[,] rooms = {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        private static (int Row, int Column) Location = (1,1);

        private static bool Move(Commands command)
        {
            bool canMove = false;

            switch (command)
            {
                case Commands.NORTH when Location.Row < rooms.GetLength(0) - 1:
                    Location.Row++;
                    canMove = true;
                    break;

                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    canMove = true;
                    break;

                case Commands.EAST when Location.Column < rooms.GetLength(1) - 1:
                    Location.Column++;
                    canMove = true;
                    break;

                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    canMove = true;
                    break;
            }

            return canMove;
        }

    }
}