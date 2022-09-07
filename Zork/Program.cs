using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            bool isRunning = true;
            
            while (isRunning == true)
            {
                Console.Write($"{rooms[currentRoom]}\n> ");
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
                        outputString = "This is an open field west of a white house, with a boarded front door. \nA rubber mat saying 'Welcome to Zork!' lies by the door.";
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

        private static string[] rooms = { "Forest", "West of House", "Behind House", "Clearing", "Canyon View" };

        private static int currentRoom = 1;

        private static bool Move(Commands command)
        {
            bool canMove = false;

            switch (command)
            {
                case Commands.NORTH:
                case Commands.SOUTH:
                    break;

                case Commands.EAST when currentRoom < rooms.Length - 1:
                    currentRoom++;
                    canMove = true;
                    break;

                case Commands.WEST when currentRoom > 0:
                    currentRoom--;
                    canMove = true;
                    break;
            }

            return canMove;
        }
    }
}