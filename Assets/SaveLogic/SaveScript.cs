using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveLogic
{
    public static class SaveScript
    {
        private static string pathRooms = Application.persistentDataPath + "/rooms.sav";
        private static string pathMoney = Application.persistentDataPath + "/money.sav";
        private static string pathReputation = Application.persistentDataPath + "/reputation.sav";
        private static string pathScenes = Application.persistentDataPath + "/scenes.sav";

        
        public static void SaveMoney(int money)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            FileStream stream = new FileStream(pathMoney, FileMode.Create);
            
            formatter.Serialize(stream, money);
            stream.Close();
        }
        
        public static void SaveReputation(int reputation)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            FileStream stream = new FileStream(pathReputation, FileMode.Create);
            
            formatter.Serialize(stream, reputation);
            stream.Close();
        }
        
        public static void SaveScenes(bool[] scenes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            FileStream stream = new FileStream(pathScenes, FileMode.Create);
            
            formatter.Serialize(stream, scenes);
            stream.Close();
        }
        
        public static void SaveRooms(bool[] roomsUnlocked)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            FileStream stream = new FileStream(pathRooms, FileMode.Create);
            
            formatter.Serialize(stream, roomsUnlocked);
            stream.Close();
        }

        public static int LoadMoney()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(pathMoney))
            {
                FileStream stream = new FileStream(pathMoney, FileMode.Open);

                int money = (int) formatter.Deserialize(stream);
                stream.Close();
                return money;
            }

            Debug.Log("Save file not found " + pathMoney);
            throw new FileNotFoundException();
        }
        
        public static int LoadReputation()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(pathReputation))
            {
                FileStream stream = new FileStream(pathReputation, FileMode.Open);

                int reputation = (int) formatter.Deserialize(stream);
                stream.Close();
                return reputation;
            }

            Debug.Log("Save file not found " + pathMoney);
            throw new FileNotFoundException();
        }
        
        public static bool[] LoadScenes()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(pathScenes))
            {
                FileStream stream = new FileStream(pathScenes, FileMode.Open);

                bool[] scenes = (bool[])formatter.Deserialize(stream);
                stream.Close();
                return scenes;
            }
            
            Debug.Log("Save file not found " + pathScenes);
            throw new FileNotFoundException();
        }
        
        public static bool[] LoadRooms()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(pathRooms))
            {
                FileStream stream = new FileStream(pathRooms, FileMode.Open);

                bool[] rooms = (bool[]) formatter.Deserialize(stream);
                stream.Close();
                return rooms;
            }
            
            Debug.Log("Save file not found " + pathRooms);
            throw new FileNotFoundException();
        }

        
    }

    
}