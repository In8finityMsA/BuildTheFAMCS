using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public RoomObject roomInfo;
    public float floorHeight = 200;
    public float floorIndexWidth = 400;
    
    // Start is called before the first frame update
    void Start()
    {
        roomInfo = RoomManager.Instance.rooms[RoomManager.Instance.CurrentRoom];
        if (roomInfo == null)  {Debug.Log("Null room"); return;}
        
        GetComponent<SpriteRenderer>().size = new Vector2(roomInfo.width, roomInfo.height);
        gameObject.transform.position =
            new Vector3(floorIndexWidth * roomInfo.indexInFloor, floorHeight * roomInfo.floor);
        SetSprite();

        foreach (var character in roomInfo.characters)
        {
            Debug.Log(character);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void SetSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = roomInfo.isUnlocked ? roomInfo.constructedSprite : roomInfo.underConstructionSprite;
    }

    void OnMouseDown()
    {
        Debug.Log($"Room Clicked! floor: {roomInfo.floor}, index: {roomInfo.indexInFloor}.");
        roomInfo.isUnlocked = !roomInfo.isUnlocked;
        SetSprite();
    }
}
