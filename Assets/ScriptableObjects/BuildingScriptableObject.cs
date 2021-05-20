using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 3)]
public class BuildingScriptableObject : ScriptableObject
{
    public string name;
    public List<FloorScriptableObject> floors;
    public Vector3 startPosition;
}
