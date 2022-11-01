using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Zork
{
    public class World
    {
        public Room[] Rooms { get; }

        [JsonIgnore]
        public Dictionary<string, Room> RoomsByName => mRoomsByName;

        [JsonProperty]
        private string StartingLocation { get; set; }

        private Dictionary<string, Room> mRoomsByName;

        public Item[] Items { get; }

        [JsonIgnore]
        public Dictionary<string, Item> ItemsByName { get; }

        public Player SpawnPlayer() => new Player(this, StartingLocation);

        public World(Room[] rooms, Item[] items)
        {
            Rooms = rooms;
            mRoomsByName = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
            foreach (Room room in rooms)
            {
                mRoomsByName.Add(room.Name, room);
            }

            Items = items;
            ItemsByName = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
            foreach (Item item in items)
            {
                ItemsByName.Add(item.Name, item);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            mRoomsByName = Rooms.ToDictionary(room => room.Name, room => room);

            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
                room.UpdateInventory(this);
            }
        }
    }
}
