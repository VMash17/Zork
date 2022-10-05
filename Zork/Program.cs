using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    class Program
    {
        private static Room CurrentRoom
        { 
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }

        static void Main(string[] args)
        {
            InitializeRooms(@"Content\Rooms.json");

            Console.WriteLine("Welcome to Zork!");

            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning == true)
            {
                Console.WriteLine(CurrentRoom);
                if (previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }
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

        private static Room[,] Rooms;
        private static void InitializeRooms(string roomsFileName)
        {
            Rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFileName));
        }

        private static (int Row, int Column) Location = (1, 1);

        static bool Move(Commands command)
        {
            bool canMove = true;
            switch (command)
            {
                case Commands.NORTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;

                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    break;

                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;

                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    break;

                default:
                    canMove = false;
                    break;
            }
            return canMove;
        }
    }
}