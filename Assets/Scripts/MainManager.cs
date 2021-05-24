using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using SaveLogic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MainManager Instance { get; set; }
    
    public List<RoomScriptableObject> roomsList;
    private Dictionary<RoomScriptableObject, int> rooms = new Dictionary<RoomScriptableObject, int>();
    public List<string> scenesList = new List<string>() {"BugScene", "CheatScene", "LatenessScene", "ServerScene", "WordScene"};
    private Dictionary<string, int> scenes = new Dictionary<string, int>();
    
    public bool useSaves;
    public bool resetSavesOnStartup;
    public int defaultMoney;
    
    private int money;
    private bool[] roomsUnlocked;
    private bool[] scenesCompleted;

    public int Money
    {
        get => money;
        set
        {
            SaveScript.SaveMoney(value);
            money = value;
        }
    }
    
    public void SetRoomUnlocked(RoomScriptableObject room, bool unlockState)
    {
        if (rooms.ContainsKey(room))
        {
            room.isUnlocked = unlockState;
            roomsUnlocked[rooms[room]] = unlockState;
            SaveScript.SaveRooms(roomsUnlocked);
        }
    }

    /**
     * Don't use it because of dictionaty iteration by value
     */
    public void SetRoomUnlocked(int index, bool unlockState)
    {
        if (index < roomsUnlocked.Length)
        {
            roomsUnlocked[index] = unlockState;
            SaveScript.SaveRooms(roomsUnlocked);
            foreach (var pair in rooms)
            {
                if (pair.Value == index)
                {
                    pair.Key.isUnlocked = unlockState;
                    break;
                }
            }
        }
    }

    public void SetSceneCompleted(string scene, bool completedState)
    {
        if (scenes.ContainsKey(scene))
        {
            scenesCompleted[scenes[scene]] = completedState;
            SaveScript.SaveScenes(scenesCompleted);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        roomsUnlocked = new bool[roomsList.Count];
        money = defaultMoney;
        scenesCompleted = new bool[roomsList.Count];

        if (resetSavesOnStartup)
        {
            Save();
        }
        
        if (useSaves)
        {
            try
            {
                money = SaveScript.LoadMoney();
                roomsUnlocked = SaveScript.LoadRooms();
                scenesCompleted = SaveScript.LoadScenes();
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Save file not found");
            }
        }


        int index = 0;
        foreach (var room in roomsList)
        {
            rooms.Add(room, index);
            room.isUnlocked = roomsUnlocked[index++];
        }

        index = 0;
        foreach (var scene in scenesList)
        {
            scenes.Add(scene, index);
        }
    }

    private void OnApplicationQuit()
    {
        if (useSaves)
        {
            Save();
        }
    }

    void Save()
    {
        SaveScript.SaveMoney(money);
        SaveScript.SaveRooms(roomsUnlocked);
        SaveScript.SaveScenes(scenesCompleted);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
