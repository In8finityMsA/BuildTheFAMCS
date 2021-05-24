using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 4)]
public class CharacterScriptableObject : ScriptableObject
{

    public string name;
    public string jsonFilename;
    public Sprite sprite;
    public List<string> minigameSceneNames;
    public bool hasQuest;

}
