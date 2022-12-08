using System;
using System.Collections.Generic;

namespace Zork.Common
{
   [Serializable]
   public class PlayerData
    {
        public Room startingLocation;
        public int score;
        public int moves;
        public IEnumerable<Item> inventory;

        public PlayerData(Player player, World world)
        {
            startingLocation = player.CurrentRoom;
            score = player.Score;
            moves = player.Moves;
            inventory = player.Inventory;
        }
    }
}
