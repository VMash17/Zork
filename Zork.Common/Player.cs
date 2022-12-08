using System.Collections.Generic;
using System;

namespace Zork.Common
{
    public enum Directions
    {
        North = Commands.NORTH,
        South = Commands.SOUTH,
        East = Commands.EAST,
        West = Commands.WEST,
        Up = Commands.UP,
        Climb = Commands.CLIMB,
        Down = Commands.DOWN
    }
    public class Player
    {
        private int moves;
        private int score = 0;

        public event EventHandler<int> MovesChanged;
        public event EventHandler<int> ScoreChanged;
        public event EventHandler<Room> LocationChanged;

        public Room CurrentRoom
        {
            get => _currentRoom;
            set
            {
                if (_currentRoom != value)
                {
                    _currentRoom = value;
                    LocationChanged?.Invoke(this, _currentRoom);
                }
            }
        }

        public IEnumerable<Item> Inventory => _inventory;

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            _inventory = new List<Item>();
        }

        public int Moves
        {
            get => moves;
            set
            {
                if (moves != value)
                {
                    moves = value;
                    MovesChanged?.Invoke(this, moves);
                }
            }
        }

        public int Score
        {
            get => score;
            set
            {
                if (score != value)
                {
                    score = value;
                    ScoreChanged?.Invoke(this, score);
                }
            }
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
            }

            return didMove;
        }

        public int ReturnScore()
        {
            return score;
        }

        public int ReturnMoves()
        {
            return moves+1;
        }

        public void AddToInventory(Item item)
        {
            if (_inventory.Contains(item))
            {
                throw new Exception($"Item {item} already exists in inventory.");
            }
            _inventory.Add(item);
        }

        public void RemoveFromInventory(Item itemToDrop)
        {
            if (_inventory.Remove(itemToDrop) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
        }

        private readonly World _world;
        private Room _currentRoom;
        private readonly List<Item> _inventory;
    }
}
