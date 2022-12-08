using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zork.Common
{
    public class SaveSystem
    {
        public static void SavePlayer(Player player, World world)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = @"player";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player, world);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerData LoadPlayer()
        {
            string path = @"player";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                return data;
            }
            else
            {
                throw new Exception("Save file not found in " + path);
            }
        }
    }
}
