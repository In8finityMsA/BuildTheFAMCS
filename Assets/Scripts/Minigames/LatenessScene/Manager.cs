using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public int penalty;
    public int reward;
    
    public GameObject BaffPrefab;
    public GameObject DebaffPrefab;

    public Button EndButton;

    public int CountBaffs;
    public int CountDebaffs;
    
    public int BaffsToWin;

    public bool IsWin = false;
    public bool IsLose = false;
    private bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        RoomScriptableObject room = Resources.Load<RoomScriptableObject>("Room11.asset");
        if (room != null)
        {
            Debug.Log("Not null");
            Debug.Log(room.characters[0].name);
        }
        else
        {
            Debug.Log("Null");
        }
        EndButton.gameObject.SetActive(false);

        BaffsToWin = CountBaffs;

        for (int i = 0; i < CountBaffs; i++)
        {
            Instantiate(BaffPrefab, new Vector2(Random.Range(-9f, 4f), Random.Range(-3f, 1f)), Quaternion.identity);
        }
        for (int i = 0; i < CountDebaffs; i++)
        {
            Instantiate(DebaffPrefab, new Vector2(Random.Range(-9f, 4f), Random.Range(-3f, 1f)), Quaternion.identity);
        }
    }

    void StepForWin()
    {
        BaffsToWin --;
    }

    void Lose()
    {
        IsLose = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (BaffsToWin == 0 && !IsLose)
        {
            MainManager.Instance.Money += reward;
            Debug.Log(gameObject.scene.name);
            MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);
            IsWin = true;
            EndButton.GetComponentInChildren<Text>().text = "You have won!";
            EndButton.onClick.AddListener(OnClick);
            EndButton.gameObject.SetActive(true);
        }
        else if (IsLose && !IsWin)
        {
            MainManager.Instance.Money -= penalty;
            Debug.Log(gameObject.scene.name);
            MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);
            EndButton.GetComponentInChildren<Text>().text = "You have lost!";
            EndButton.onClick.AddListener(OnClick);
            EndButton.gameObject.SetActive(true);
        }
    }

    void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            SceneManager.LoadScene("MainScene");
        }
    }
    
}
