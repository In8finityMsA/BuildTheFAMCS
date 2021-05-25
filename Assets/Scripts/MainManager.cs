using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using DefaultNamespace;
using SaveLogic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MainManager Instance { get; set; }

    public List<RoomScriptableObject> roomsList;
    private Dictionary<RoomScriptableObject, int> rooms = new Dictionary<RoomScriptableObject, int>();
    public List<string> scenesList = new List<string>() {"BugsScene", "CheatScene", "LatenessScene", "ServerScene", "WordScene"};
    private Dictionary<string, int> scenes = new Dictionary<string, int>();

    public static bool newGame;
    
    public bool useSaves;
    public bool resetSavesOnStartup;
    
    public int defaultMoney;
    public int defultReputation;
    public bool[] defaultRooms = new bool[8];
    public bool[] defaultScenes = new bool[5];
    
    private int money;
    private int reputation;
    private bool[] roomsUnlocked;
    private bool[] scenesCompleted;
    
    //Dialogs
    private int currentIndex;

    public GameObject dialogWindow;
    public GameObject darkTint;
    
    public GameObject textObject;
    
    public GameObject rightButtonObject;
    public GameObject leftButtonObject;
    public GameObject bigButtonObject;
    
    public GameObject rightButtonTextObject;
    public GameObject leftButtonTextObject;
    public GameObject bigButtonTextObject;
    
    
    private Text text;
    private Button rightButton;
    private Button leftButton;
    private Button bigButton;
    private Text rightButtonText;
    private Text leftButtonText;
    private Text bigButtonText;

    private List<DialogPart> dialog;
    private List<Button> buttons = new List<Button>();
    
    public delegate void MoneyDelegate(int money);
    public event MoneyDelegate OnMoneyChange;
    
    public delegate void ReputationDelegate(int reputation);
    public event ReputationDelegate OnReputationChange;

    public int Money
    {
        get => money;
        set
        {
            SaveScript.SaveMoney(value);
            money = value;
            OnMoneyChange?.Invoke(money);
        }
    }
    
    public int Reputation
    {
        get => reputation;
        set
        {
            SaveScript.SaveReputation(value);
            reputation = value;
            OnReputationChange?.Invoke(reputation);
        }
    }
    
    public void SetRoomUnlocked(RoomScriptableObject room, bool unlockState)
    {
        if (rooms.ContainsKey(room))
        {
            room.isUnlocked = unlockState;
            roomsUnlocked[rooms[room]] = unlockState;
            SaveScript.SaveRooms(roomsUnlocked);
        }
        else
        {
            Debug.Log("No such room found");
        }
    }

    /**
     * Don't use it because of dictionaty iteration by value
     */
    public void SetRoomUnlocked(int index, bool unlockState)
    {
        if (index < roomsUnlocked.Length)
        {
            roomsUnlocked[index] = unlockState;
            SaveScript.SaveRooms(roomsUnlocked);
            foreach (var pair in rooms)
            {
                if (pair.Value == index)
                {
                    pair.Key.isUnlocked = unlockState;
                    break;
                }
            }
        }
        else
        {
            Debug.Log("No such index found");
        }
    }
    
    public bool GetRoomStatus(RoomScriptableObject room)
    {
        if (rooms.ContainsKey(room))
        {
            return roomsUnlocked[rooms[room]];
        }
        else
        {
            Debug.Log("No such room found");
            throw new ArgumentException();
        }
    }

    public void SetSceneCompleted(string scene, bool completedState)
    {
        if (scenes.ContainsKey(scene))
        {
            scenesCompleted[scenes[scene]] = completedState;
            SaveScript.SaveScenes(scenesCompleted);
        }
        else
        {
            Debug.Log("No such scene found");
        }
    }

    public bool GetSceneStatus(string scene)
    {
        if (scenes.ContainsKey(scene))
        {
            return scenesCompleted[scenes[scene]];
        }
        else
        {
            Debug.Log("No such scene found");
            throw new ArgumentException();
        }
    }

    private void Awake()
    {
        Instance = this;
        
        roomsUnlocked = new bool[roomsList.Count];
        money = defaultMoney;
        scenesCompleted = new bool[scenesList.Count];

        if (newGame)
        {
            ResetSaves();
        }
        
        
        if (useSaves)
        {
            try
            {
                Money = SaveScript.LoadMoney();
                Reputation = SaveScript.LoadMoney();
                roomsUnlocked = SaveScript.LoadRooms();
                scenesCompleted = SaveScript.LoadScenes();
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Save file not found");
            }
        }


        int index = 0;
        foreach (var room in roomsList)
        {
            rooms.Add(room, index);
            room.isUnlocked = roomsUnlocked[index++];
        }

        index = 0;
        foreach (var scene in scenesList)
        {
            scenes.Add(scene, index++);
        }

        OnMoneyChange += EndGame;
        OnReputationChange += EndGame;
        
        //Debug
        var darkTintButton = darkTint.GetComponent<Button>();
        darkTintButton.onClick.AddListener(() => EndDialog());

        text = textObject.GetComponent<Text>();
        
        rightButton = rightButtonObject.GetComponent<Button>();
        leftButton = leftButtonObject.GetComponent<Button>();
        bigButton = bigButtonObject.GetComponent<Button>();
        
        rightButtonText = rightButtonTextObject.GetComponent<Text>();
        leftButtonText = leftButtonTextObject.GetComponent<Text>();
        bigButtonText = bigButtonTextObject.GetComponent<Text>();
        
        
        buttons.Add(leftButton);
        buttons.Add(rightButton);
        buttons.Add(bigButton);
        
        dialogWindow.SetActive(false);
        darkTint.SetActive(false);

        

    }

    private void Start()
    {
        if (newGame)
        {
            var dialogArray = JsonLoader.GetJsonArrayFromFile("StartScript.json");
            newGame = false;
            StartDialog(dialogArray);
        }
    }

    public void StartDialog(List<DialogPart> dialog)
    {
        this.dialog = dialog;
        
        dialogWindow.SetActive(true);
        darkTint.SetActive(true);
        currentIndex = 0;
        CameraMove.isCameraBlocked = true;
        DisplayPart(GetCurrentPart());

    }

    private void DisplayPart(DialogPart dialogPart)
    {
        ResetVisibiltyAndListeneres();
        
        //rightButton.onClick.AddListener(() => Debug.Log("Clicked!!!"));
        text.text = dialogPart.text;
        if (dialogPart.replies.Count == 0)
        {
            rightButtonObject.SetActive(true);

            rightButtonText.text = "Далее";

            rightButton.onClick.AddListener(() => SetCurrent(dialogPart.nextIndices[0]));
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
                rightButton.onClick.AddListener(() => SetCurrent(dialogPart.nextIndices[1]));
            }
        }
        CheckActions(dialogPart);
    }

    private void CheckActions(DialogPart dialogPart)
    {
        int buttonIndex = 0;
        Debug.Log($"Current Index: {currentIndex}.");
        foreach (var actionString in dialogPart.actions)
        {
            Debug.Log($"Action: {actionString}, butIndex {buttonIndex}");
            string[] actionWords = actionString.Split(' ');

            if (actionWords.Length >= 1)
            {
                switch (actionWords[0])
                {
                    case "scene":
                    {
                        if (actionWords.Length >= 2)
                        {
                            buttons[buttonIndex].onClick.AddListener(() => SetScene(actionWords[1]));
                        }
                        else
                        {
                            Debug.Log("Not enough parameters for action " + actionWords[0]);
                        }

                        break;
                    }
                    case "buy":
                    {
                        if (actionWords.Length >= 3)
                        {
                            if (Int32.TryParse(actionWords[1], out var price) &&
                                Int32.TryParse(actionWords[2], out var index))
                            {
                                if (Money >= price)
                                {
                                    Money -= price;
                                    var part = new DialogPart("С приобритением вас!", new List<string>(), new List<int>(){0}, new List<string>(){"", "close", ""});
                                    foreach (var pair in rooms)
                                    {
                                        if (pair.Value == index)
                                        {
                                            pair.Key.script.RoomUnlock();
                                            break;
                                        }
                                    }

                                    buttons[buttonIndex].onClick.AddListener(() => DisplayPart(part));
                                }
                                else
                                {
                                    var part = new DialogPart("Недостаточно средств", new List<string>(), new List<int>(){0}, new List<string>(){"", "close", ""});
                                    buttons[buttonIndex].onClick.AddListener(() => DisplayPart(part));
                                }
                            }
                        }
                        break;
                    }
                    case "reputation":
                    {
                        break;
                    }
                    case "close":
                    {
                        buttons[buttonIndex].onClick.AddListener(() => EndDialog());
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("No words in actions");
            }

            buttonIndex++;
        }
    }

    private void SetCurrent(int index)
    {
        currentIndex = index;
        DisplayPart(GetCurrentPart());
    }

    private void SetScene(string scene)
    {
        SceneManager.LoadScene(scene);
        EndDialog();
    }
    
    private DialogPart GetCurrentPart()
    {
        if (currentIndex >= dialog.Count)
        {
            Debug.Log($"Index for dialog is to big. Index: {currentIndex}, Length: {dialog.Count}");
            return null;
        }
        else
        {
            return dialog[currentIndex];
        }
    }

    public void EndDialog()
    {
        ResetVisibiltyAndListeneres();
        CameraMove.isCameraBlocked = false;
        dialogWindow.SetActive(false);
        darkTint.SetActive(false);
        currentIndex = 0;
        dialog = null;
    }
    
    private void ResetVisibiltyAndListeneres()
    {
        rightButtonObject.SetActive(false);
        leftButtonObject.SetActive(false);
        bigButtonObject.SetActive(false);
        rightButton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
        bigButton.onClick.RemoveAllListeners();
    }

    private void OnApplicationQuit()
    {
        if (useSaves)
        {
            Save();
        }
    }

    void Save()
    {
        SaveScript.SaveMoney(money);
        SaveScript.SaveReputation(reputation);
        SaveScript.SaveRooms(roomsUnlocked);
        SaveScript.SaveScenes(scenesCompleted);
    }

    private void ResetSaves()
    {
        SaveScript.SaveMoney(defaultMoney);
        SaveScript.SaveReputation(defultReputation);
        SaveScript.SaveRooms(defaultRooms);
        SaveScript.SaveScenes(defaultScenes);

    }

    private void EndGame(int param)
    {
        if (Money == 0)
        {
            var dialogArray = JsonLoader.GetJsonArrayFromFile("LowBudgetScript.json");
            StartDialog(dialogArray);
            SceneManager.LoadScene("MainMenu");
            OnMoneyChange = null;
            OnReputationChange = null;
        }
        else if (Reputation == 0)
        {
            var dialogArray = JsonLoader.GetJsonArrayFromFile("LowRepScript.json");
            StartDialog(dialogArray);
            SceneManager.LoadScene("MainMenu");
            OnMoneyChange = null;
            OnReputationChange = null;
        }
        
    }

    [System.Serializable]
    public class DialogTest
    {
        public List<string> text;
    }
    
    [System.Serializable]
    public class DialogPart
    {
        public string text;
        public List<string> replies;
        public List<int> nextIndices;
        public List<string> actions;
        //public bool hasScene;

        public DialogPart()
        {
        }

        public DialogPart(string text, List<string> replies, List<int> nextIndices, List<string> actions)
        {
            this.text = text;
            this.replies = replies;
            this.nextIndices = nextIndices;
            this.actions = actions;
            //this.hasScene = hasScene;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
