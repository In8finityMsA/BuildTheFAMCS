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
    public Dictionary<RoomScriptableObject, int> rooms = new Dictionary<RoomScriptableObject, int>();
    private int money;
    private bool[] roomsUnlocked;
    public SaveScript.SaveDataScenes scenesData;

    public bool useSaves;
    public bool resetSavesOnStartup;
    
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

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        roomsUnlocked = new bool[roomsList.Count];
        money = 100;
        scenesData = new SaveScript.SaveDataScenes();

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
                scenesData = SaveScript.LoadScenes();
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Save file not found");
            }
        }


        int i = 0;
        foreach (var room in roomsList)
        {
            rooms.Add(room, i);
            room.isUnlocked = roomsUnlocked[i++];
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
        SaveScript.SaveScenes(scenesData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
