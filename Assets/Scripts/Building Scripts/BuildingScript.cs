using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingScript : MonoBehaviour
{

    public BuildingScriptableObject buildingInfo;
    public GameObject floorPrefab;
    public GameObject dialogPanel;
    public GameObject darkTint;
    void Start()
    {
        if (gameObject.transform.position == Vector3.zero)
            gameObject.transform.position = buildingInfo.startPosition;
        
        float currentHeight = 0;
        uint floorAmount = 0;
        foreach (var floor in buildingInfo.floors)
        {
            var floorObject = Instantiate(floorPrefab, new Vector3(), Quaternion.identity);
            floor.floorNumber = floorAmount++;
            //Warning: All rooms in the floor must have the same height
            //currentHeight += floor.rooms[0] != null ? floor.rooms[0].underConstructionSprite.bounds.size.y * roomPrefab.transform.localScale.y : 0; 
            
            
            floorObject.transform.SetParent(gameObject.transform);
            floorObject.transform.localPosition = new Vector3(floor.firstRoomShift, currentHeight + floor.rooms[0].constructedSprite.bounds.size.y, 0);
            currentHeight += floor.rooms[0].constructedSprite.bounds.size.y; 

            floorObject.GetComponent<FloorScript>().Init(floor);
        }
        
    }

}
