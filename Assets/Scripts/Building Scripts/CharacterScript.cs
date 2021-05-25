using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CharacterScript : MonoBehaviour
{
    private CharacterScriptableObject characterInfo;
    private bool isInit = false;
    private List<MainManager.DialogPart> dialog;


    internal void Init(CharacterScriptableObject character)
    {
        if (isInit) return;
        
        isInit = true;
        characterInfo = character;
        characterInfo.script = this;
        
        SetSprite();

        dialog = JsonLoader.GetJsonArrayFromFile(characterInfo.jsonFilename);

        //Only for testing
        if (dialog == null)
        {
            dialog.Add(new MainManager.DialogPart("testText0", new List<string>() {"Next", "After Next"},
                new List<int>() {1, 2}, new List<string>() {"", "", ""}));
        }
        /*dialog.Add(new MainManager.DialogPart("testText1", new List<string>(){}, new List<int>(){2}, new List<string>(){"", "", ""}));
        dialog.Add(new MainManager.DialogPart("testText2", new List<string>(){"Mid"}, new List<int>(){3}, new List<string>(){"", "", ""}));
        dialog.Add(new MainManager.DialogPart("testText3", new List<string>(){"ToBegin", "ToLast(Scene)"}, new List<int>(){0, 3}, new List<string>(){"", "scene BugsScene", ""}));*/
    }

    private void IsNull(string name, GameObject checkObject)
    {
        Debug.Log(name + "is " + (checkObject == null ? "null" : checkObject.ToString()));
    }
    
    void SetSprite()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterInfo.sprite;
        
        //Collider disabled for now because now clicks are handled on room collider
        /*var boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.size = boxCollider.transform.InverseTransformVector(spriteRenderer.bounds.size);*/
    }

    public void StartDialog()
    {
        MainManager.Instance.StartDialog(dialog);

    }

    [System.Serializable]
    public class DialogArray
    {
        public MainManager.DialogPart[] array;
    }
}
