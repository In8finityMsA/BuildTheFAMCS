using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    
    //singletone
    /*public static RoomManager Instance { get; private set; }
    public GameObject roomPrefab;
    public RoomScriptableObject roomObject;
    public List<RoomScriptableObject> rooms;
    private int currentRoom = 0;*/

    //public int CurrentRoom => currentRoom++;

    private void Awake()
    {
        /*Instance = this;
        rooms.Add(roomObject);
        Debug.Log("Manager awake");*/
    }

    // Start is called before the first frame update
    void Start()
    {
        /*var room = Instantiate(roomPrefab, new Vector3(), Quaternion.identity);
        Debug.Log("Manager start");*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
