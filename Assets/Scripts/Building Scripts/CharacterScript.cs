using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterScript : MonoBehaviour
{
    private CharacterScriptableObject characterInfo;
    private bool isInit = false;
    private List<DialogPart> dialog;

    internal void Init(CharacterScriptableObject character)
    {
        if (isInit) return;
        
        isInit = true;
        characterInfo = character;
        SetSprite();

        var jsonRelativeFilename = Application.dataPath + "/" + characterInfo.jsonFilename;
        if (File.Exists(jsonRelativeFilename))
        {
            using (var reader = new StreamReader(jsonRelativeFilename))
            {
                string jsonString = reader.ReadToEnd();
                dialog = JsonUtility.FromJson<List<DialogPart>>(jsonString);
            }
            
        }
        else
        {
            Debug.Log("No dialog file found: " + jsonRelativeFilename);
            //throw new FileNotFoundException();
        }
    }
    
    void SetSprite()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterInfo.sprite;
        
        var boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.size = boxCollider.transform.InverseTransformVector(spriteRenderer.bounds.size);
    }

    private void OnMouseDown()
    {
        if (characterInfo.minigameSceneNames.Count > 0)
        {
            var minigame = characterInfo.minigameSceneNames[0];
            if (!MainManager.Instance.GetSceneStatus(minigame))
            {
                SceneManager.LoadScene(minigame);
            }
            else
            {
                Debug.Log("You have already done this minigame.");
            }
            
        }
        else
        {
            Debug.Log("I don't have quests for you");
        }
    }

    [System.Serializable]
    public class DialogPart
    {
        public string text;
        public List<string> replies;
        public List<int> nextIndices;
        public Dictionary<int, int> scenesIndices;

        public DialogPart()
        {
        }

        public DialogPart(string text, List<string> replies, List<int> nextIndices, Dictionary<int, int> scenesIndices)
        {
            this.text = text;
            this.replies = replies;
            this.nextIndices = nextIndices;
            this.scenesIndices = scenesIndices;
        }
    }
}
