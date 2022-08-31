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
                Console.Write("> ");
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
                        outputString = $"You moved {command}.";
                        break;
                    default:
                        outputString = "Unrecognized command.";
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
    }
}