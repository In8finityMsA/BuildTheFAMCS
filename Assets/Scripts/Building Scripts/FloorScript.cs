using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorScript : MonoBehaviour
{

    private FloorScriptableObject floorInfo;
    public GameObject roomPrefab;
    private bool isInit = false;

    internal void Init(FloorScriptableObject floor)
    {
        if (isInit) return;
        
        isInit = true;
        floorInfo = floor;

        uint roomAmount = 0;
        float currentWidth = 0;
        foreach (var room in floorInfo.rooms)
        {
            var roomObject = Instantiate(roomPrefab, new Vector3(), Quaternion.identity);
            //room.height = floorInfo.height;
            room.indexInFloor = roomAmount++;
            room.floor = floorInfo.floorNumber;
            //var width = room.underConstructionSprite.bounds.size.x * roomObject.transform.localScale.x;
            var width = room.constructedSprite.bounds.size.x;
            Debug.Log($"Sprite size x: {room.constructedSprite.bounds.size.x}, y: {room.constructedSprite.bounds.size.y}. Floor num: {floorInfo.floorNumber}, room: {roomAmount}. Current width: {currentWidth}.");
            
            roomObject.transform.SetParent(gameObject.transform);
            roomObject.transform.localPosition = new Vector3(currentWidth + room.constructedSprite.bounds.size.x / 2, 0, 0);
            currentWidth += width;
            roomObject.GetComponent<SpriteRenderer>().sortingLayerName = "Rooms";
            
            var roomScript = roomObject.GetComponent<RoomScript>();
            roomScript.Init(room);

        }
    }
    
}
