﻿using System;
using System.Collections.Generic;
using System.IO;

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
            InitializeRoomDescriptions(@"Content\Rooms.txt");

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

        private static void InitializeRoomDescriptions(string roomsFileName)
        {
            var roomMap = new Dictionary<string, Room>();

            foreach (Room room in rooms)
            {
                roomMap[room.Name] = room;
            }
            
            string[] lines = File.ReadAllLines(roomsFileName);
            foreach (string line in lines)
            {
                const string fieldDelimiter = "##";
                const int expectedFieldCount = 2;

                string[] fields = line.Split(fieldDelimiter);
                if (fields.Length != expectedFieldCount)
                {
                    throw new InvalidDataException("Invalid record.");
                }
                string name = fields[(int)Fields.Name];
                string description = fields[(int)Fields.Description];

                roomMap[name].Description = description;
            }          
        }

        private static readonly Room[,] rooms = {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        private static (int Row, int Column) Location = (1, 1);

        static bool Move(Commands command)
        {
            bool canMove = true;
            switch (command)
            {
                case Commands.NORTH when Location.Row < rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;

                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    break;

                case Commands.EAST when Location.Column < rooms.GetLength(1) - 1:
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

        private enum Fields
        {
            Name = 0,
            Description = 1
        }
    }
}