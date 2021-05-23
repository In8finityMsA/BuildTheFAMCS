using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveLogic
{
    public static class SaveScript
    {
        private static string pathRooms = Application.persistentDataPath + "/rooms.sav";
        private static string pathMoney = Application.persistentDataPath + "/money.sav";
        private static string pathScenes = Application.persistentDataPath + "/scenes.sav";
        //private static int defualtMoney = 100;
        [System.Serializable]
        public class SaveDataScenes
        {
            public string[] scenes;
            public bool[] isScenesCompleted;
        }
        
        public static void SaveMoney(int money)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            FileStream stream = new FileStream(pathMoney, FileMode.Create);
            
            formatter.Serialize(stream, money);
            stream.Close();
        }
        
        public static void SaveScenes(SaveDataScenes scenes)
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
        
        public static SaveDataScenes LoadScenes()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(pathScenes))
            {
                FileStream stream = new FileStream(pathScenes, FileMode.Open);

                SaveDataScenes scenes = formatter.Deserialize(stream) as SaveDataScenes;
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