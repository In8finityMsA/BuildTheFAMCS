using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Floor", menuName = "ScriptableObjects/Floor", order = 2)]
public class FloorScriptableObject : ScriptableObject
{
    //Rooms' sprites' height in the same floor must be the same;
    public List<RoomScriptableObject> rooms;
    internal uint floorNumber;
    //public uint height;
    public int firstRoomShift;

}
