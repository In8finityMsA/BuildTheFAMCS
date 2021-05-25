using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CharacterScript : MonoBehaviour
{
    private CharacterScriptableObject characterInfo;
    private bool isInit = false;
    private List<DialogPart> dialog = new List<DialogPart>();
    private int currentIndex;

    private GameObject dialogWindow;
    private GameObject darkTint;
    private GameObject textObject;
    private GameObject rightButtonObject;
    private GameObject leftButtonObject;
    private GameObject bigButtonObject;
    private Text text;
    private Button rightButton;
    private Button leftButton;
    private Button bigButton;
    private Text rightButtonText;
    private Text leftButtonText;
    private Text bigButtonText;
    

    internal void Init(CharacterScriptableObject character)
    {
        if (isInit) return;
        
        isInit = true;
        characterInfo = character;
        characterInfo.script = this;
        var canvasObject = GameObject.Find("Canvas");
        dialogWindow = GameObject.Find("Panel");
        darkTint = GameObject.Find("DarkTint");
        textObject = GameObject.Find("Text");
        rightButtonObject = GameObject.Find("RightButton");
        leftButtonObject = GameObject.Find("LeftButton");
        bigButtonObject = GameObject.Find("BigButton");
        var rightButtonTextObject = GameObject.Find("RightButtonText");
        var leftButtonTextObject = GameObject.Find("LeftButtonText");
        var bigButtonTextObject = GameObject.Find("BigButtonText");
        IsNull("Panel", dialogWindow);
        IsNull("Text", textObject);
        IsNull("Right", rightButtonObject);
        IsNull("Left", leftButtonObject);
        IsNull("Big", bigButtonObject);
        text = textObject.GetComponent<Text>();
        rightButton = rightButtonObject.GetComponent<Button>();
        leftButton = leftButtonObject.GetComponent<Button>();
        bigButton = bigButtonObject.GetComponent<Button>();
        rightButtonText = rightButtonTextObject.GetComponent<Text>();
        leftButtonText = leftButtonTextObject.GetComponent<Text>();
        bigButtonText = bigButtonTextObject.GetComponent<Text>();
        

        SetSprite();

        /*var jsonRelativeFilename = Application.dataPath + "/" + characterInfo.jsonFilename;
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
        }*/
        
        //Only for testing
        dialog.Add(new DialogPart("testText0", new List<string>(){"Next", "After Next"}, new List<int>(){1, 2}, false));
        dialog.Add(new DialogPart("testText1", new List<string>(){}, new List<int>(){2}, false));
        dialog.Add(new DialogPart("testText2", new List<string>(){"Mid"}, new List<int>(){3}, false));
        dialog.Add(new DialogPart("testText3", new List<string>(){"ToBegin", "ToLast(Scene)"}, new List<int>(){0, 3}, true));
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
        dialogWindow.SetActive(true);
        darkTint.SetActive(true);
        currentIndex = 0;
        CameraMove.isCameraBlocked = true;
        DisplayPart(GetCurrentPart());

    }

    private void DisplayPart(DialogPart dialogPart)
    {
        rightButtonObject.SetActive(false);
        leftButtonObject.SetActive(false);
        bigButtonObject.SetActive(false);
        rightButton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
        bigButton.onClick.RemoveAllListeners();
        
        //rightButton.onClick.AddListener(() => Debug.Log("Clicked!!!"));
        text.text = dialogPart.text;
        if (dialogPart.replies.Count == 0)
        {
            rightButtonObject.SetActive(true);

            rightButtonText.text = "Далее";
            if (dialogPart.hasScene)
            {
                rightButton.onClick.AddListener(SetScene);
            }
            else
            {
                rightButton.onClick.AddListener(() => SetCurrent(dialogPart.nextIndices[0]));
            }
        }
        else
        {
            if (dialogPart.replies.Count == 1)
            {
                bigButtonObject.SetActive(true);
                
                bigButtonText.text = dialogPart.replies[0];
                bigButton.onClick.AddListener(() => SetCurrent(dialogPart.nextIndices[0]));
            }
            else if (dialogPart.replies.Count == 2)
            {
                rightButtonObject.SetActive(true);
                leftButtonObject.SetActive(true);

                leftButtonText.text = dialogPart.replies[0];
                leftButton.onClick.AddListener(() => SetCurrent(dialogPart.nextIndices[0]));
                rightButtonText.text = dialogPart.replies[1];
                if (dialogPart.hasScene)
                {
                    rightButton.onClick.AddListener(SetScene);
                }
                else
                {
                    rightButton.onClick.AddListener(() => SetCurrent(dialogPart.nextIndices[1]));
                }
            }
        }
    }

    private void SetCurrent(int index)
    {
        currentIndex = index;
        DisplayPart(GetCurrentPart());
    }

    private void SetScene()
    {
        CameraMove.isCameraBlocked = false;
        if (characterInfo.minigameSceneNames != null && characterInfo.minigameSceneNames.Count > 0)
            SceneManager.LoadScene(characterInfo.minigameSceneNames[0]);
        else
        {
            Debug.Log("There is no scene!");
        }
    }

    private void EndDialog()
    {
    }

    private DialogPart GetCurrentPart()
    {
        return dialog[currentIndex];
    }

    /*void OnMouseDown()
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
    }*/

    [System.Serializable]
    public class DialogPart
    {
        public string text;
        public List<string> replies;
        public List<int> nextIndices;
        public bool hasScene;

        public DialogPart()
        {
        }

        public DialogPart(string text, List<string> replies, List<int> nextIndices, bool hasScene)
        {
            this.text = text;
            this.replies = replies;
            this.nextIndices = nextIndices;
            this.hasScene = hasScene;
        }
    }
}
