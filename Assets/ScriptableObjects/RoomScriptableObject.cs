using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "ScriptableObjects/Room", order = 1)]
public class RoomScriptableObject : ScriptableObject
{
    //underConstructionSprite and constructedSprite must be the same size;
    //Also sprite height in the same floor must be the same;
    public List<CharacterScriptableObject> characters;
    public List<Vector2> charactersPositions; //Doesn't work. For some reason using it leads to character spawn in odd position
    public bool isUnlocked = false;
    public int costToBuild;
    public Sprite underConstructionSprite;
    public Sprite constructedSprite;
    //internal float height;
    //public float width; 
    internal uint floor;
    internal uint indexInFloor;
}
