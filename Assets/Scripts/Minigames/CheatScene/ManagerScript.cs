using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    
    //in inspector
    public GameObject prefab;

    public TeacherScript teacher;
    public Rect cameraRect;

    //singletone
    public static ManagerScript Instance { get; private set; }

    public delegate void OnWinning();

    public event OnWinning HasWon;

    public int studentAmount;

    public UnityEngine.UI.Button MyButton;

    //needed to cheat for 1 student
    public const float ENOUGH = 3;

    //summarised progress
    public float CurrentProgress;

    //how often teacher starts watching
    public const int FREQUENCY = 8;

    public List<PhoneScript> students;

    //current state of teacher
    public bool teacherWatching;

    public delegate void OnTheEnd();
    public event OnTheEnd OnTheEndHandler;

    private bool isClicked = false;

    public void EndGotHim()
    {

        Clear();
        EndScript end = EndScript.Instance;
        end.gameObject.SetActive(true);
        end.btn.gameObject.GetComponentInChildren<Text>().text = "You've lost! The teacher saw you cheating!";
        end.btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);             
    }

    private void Clear()
    {
        foreach (var item in students)
        {
            item.gameObject.SetActive(false);
        }

        teacher.gameObject.SetActive(false);
    }    

    public void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            SceneManager.LoadScene("MainScene");
        }
    }

    public void EndHappened()
    {
        OnTheEndHandler += EndGotHim;
        OnTheEndHandler();
    }

    public void EndedTime()
    {
        OnTheEndHandler += EndOfTime;
        OnTheEndHandler();
    }

    public void EndOfTime()
    {
        Clear();
        EndScript end = EndScript.Instance;
        end.gameObject.SetActive(true);
        end.btn.gameObject.GetComponentInChildren<Text>().text = "You've lost! The lesson is over!";
            
        end.btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    void Awake()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        Instance = this;
        studentAmount = 2;
        PlaceStudents();
    }

    void PlaceStudents()
    {

        if (studentAmount > 4)
        {
            int first = studentAmount / 2;
            int second = studentAmount - first;
            float xCoordRow = (float)cameraRect.x + (float)cameraRect.width / 2 - first* 1.5f/ 2;
            float yCoordFirst = (float)cameraRect.y + (float)cameraRect.height / 3;
            float yCoordSecond = (float)cameraRect.y + (float)cameraRect.height / 6;

            for (int i = 0; i < first; i++)
            {
                GameObject gameOb = Instantiate(prefab, new Vector3(xCoordRow + 2 * i, yCoordFirst, 0), Quaternion.identity);
            }
            xCoordRow = (float)cameraRect.x + (float)cameraRect.width / 2 - (second) * 1.5f/ 2f ;

            for (int i = 0; i < second; i++)
            {
                GameObject gameOb = Instantiate(prefab, new Vector3(xCoordRow + 2 * i, yCoordSecond, 0), Quaternion.identity);
            }
        }
        else
        {
            float xCoordRow = (float)cameraRect.x + (float)cameraRect.width / 2 - ((float)studentAmount * 1.5f) / 2f;
            float yCoord = (float)cameraRect.y + (float)cameraRect.height / 4;
            for (int i = 0; i < studentAmount; i++)
            {
                GameObject gameOb = Instantiate(prefab, new Vector3(xCoordRow + 2 * i, yCoord, 0), Quaternion.identity);
            }
        }
    }
        

    void Start()
    {
        HasWon += UIHasWon;
        CurrentProgress = 0;
        teacherWatching = false;
        students = new List<PhoneScript>();
        
    }


    void UIHasWon()
    {
        //TODO
        Clear();
        EndScript end = EndScript.Instance;
        end.gameObject.SetActive(true);
        end.btn.gameObject.GetComponentInChildren<Text>().text = "You have won!";

        end.btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentProgress >= ENOUGH * studentAmount)
        {
            Debug.Log("WON");
            HasWon();
        }
        else if (teacherWatching)
        {
            foreach (var item in students)
            {
                if (item.isCheating)
                {
                    Debug.Log("Got you!");
                    item.Sucks();
                    teacher.TeacherAngry();                  
                }         
            }
        }
    }
}
