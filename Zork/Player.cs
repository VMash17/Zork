using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork
{
    public enum Directions
    {
        North,
        South,
        East,
        West
    }
    public class Player
    {
        private int Score = 0;
        private int Moves = 0;

        public World World { get; }

        [JsonIgnore]
        public Room Location { get; private set; }

        [JsonIgnore]
        public string LocationName
        {
            get
            {
                return Location?.Name;
            }
            set
            {
                Location = World?.RoomsByName.GetValueOrDefault(value);
            }
        }

        public List<Item> Inventory { get; }

        public Player(World world, string startingLocation)
        {
            World = world;
            LocationName = startingLocation;

            Inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool isValidMove = Location.Neighbors.TryGetValue(direction, out Room destination);
            if (isValidMove)
            {
                Location = destination;
                Moves++;
            }
            return isValidMove;
        }

        public void IncreaseScore()
        {
            Score++;
        }

        public int ReturnScore()
        {
            return Score;
        }

        public int ReturnMoves()
        {
            return Moves;
        }

        public void AddToInventory(Item item)
        {
            Inventory.Add(item);
        }

        public Item RemoveFromInventory(string itemName)
        {
            Item itemToDrop = null;

            foreach (Item item in Inventory)
            {
                if(string.Compare(item.Name, itemName, ignoreCase: true) == 0)
                {
                    itemToDrop = item;
                }
            }

            if (itemToDrop != null)
            {
                Inventory.Remove(itemToDrop);
            }
            
            return itemToDrop;
        }
    }
}
