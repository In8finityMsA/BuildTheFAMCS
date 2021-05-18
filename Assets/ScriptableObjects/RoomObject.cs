using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "ScriptableObjects/Room", order = 1)]
public class RoomObject : ScriptableObject
{
    public List<string> characters;
    public bool isUnlocked = false;
    public uint costToBuild;
    public Sprite underConstructionSprite;
    public Sprite constructedSprite;
    public uint height; //or float?
    public uint width;
    public uint floor;
    public uint indexInFloor;
}
