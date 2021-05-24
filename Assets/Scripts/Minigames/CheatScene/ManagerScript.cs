using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    public int penalty;
    public int reward;
    public bool debugThisSceneMode;
    
    //in inspector
    public GameObject prefab;

    //in inspector
    public GameObject background;

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
        if (!debugThisSceneMode)
        {
            MainManager.Instance.Money -= penalty;
            MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);
        }

        end.gameObject.SetActive(true);
        end.btn.gameObject.GetComponentInChildren<Text>().text = "Препод вас запалил(";
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
        if (!debugThisSceneMode)
        {
            MainManager.Instance.Money -= penalty;
            MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);
        }

        //displaying end button
        end.gameObject.SetActive(true);
        end.btn.gameObject.GetComponentInChildren<Text>().text = "Вы не успели списать! Пара кончилась(";    
        end.btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    void Awake()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);



        //setting background boundaries
        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector2 scale = transform.localScale;
        scale.x *= cameraSize.x / spriteSize.x;        
        scale.y *= cameraSize.y / spriteSize.y;
        background.transform.position = Vector2.zero;
        background.transform.localScale = scale;


        //for singletone
        Instance = this;

        //taking from params
        studentAmount = 2;

        PlaceStudents();
    }

    void PlaceStudents()
    {
        if (studentAmount == 1)
        {
            GameObject gameOb = Instantiate(prefab, new Vector3((float)cameraRect.x + (float)cameraRect.width / 2, (float)cameraRect.y + (float)cameraRect.height / 4, 0), Quaternion.identity);
        }
        else if (studentAmount == 2)
        {
            GameObject gameOb = Instantiate(prefab, new Vector3((float)cameraRect.x + (float)cameraRect.width / 2 - 1.75f, (float)cameraRect.y + (float)cameraRect.height / 4, 0), Quaternion.identity);
            GameObject gameOb2 = Instantiate(prefab, new Vector3((float)cameraRect.x + (float)cameraRect.width / 2 + 1.75f, (float)cameraRect.y + (float)cameraRect.height / 4, 0), Quaternion.identity);
        }
        else if (studentAmount >= 3 && studentAmount <= 4)
        {
            float xCoordRow = (float)cameraRect.x + (float)cameraRect.width / 2 - ((float)studentAmount * 3f) / 2f + 0.75f;
            float yCoord = (float)cameraRect.y + (float)cameraRect.height / 4;
            for (int i = 0; i < studentAmount; i++)
            {
                GameObject gameOb = Instantiate(prefab, new Vector3(xCoordRow + 3.5f * i, yCoord, 0), Quaternion.identity);
            }
        }
        else //normally we have from 1 to 4 students
        {
            int first = studentAmount / 2;
            int second = studentAmount - first;
            float xCoordRow = (float)cameraRect.x + (float)cameraRect.width / 2 - first* 2.5f/ 2;
            float yCoordFirst = (float)cameraRect.y + (float)cameraRect.height / 2f;
            float yCoordSecond = (float)cameraRect.y + (float)cameraRect.height / 5f;

            for (int i = 0; i < first; i++)
            {
                GameObject gameOb = Instantiate(prefab, new Vector3(xCoordRow + 3.5f * i, yCoordFirst, 0), Quaternion.identity);
            }
            xCoordRow = (float)cameraRect.x + (float)cameraRect.width / 2 - (second) * 2.5f/ 2f ;

            for (int i = 0; i < second; i++)
            {
                GameObject gameOb = Instantiate(prefab, new Vector3(xCoordRow + 3.5f * i, yCoordSecond, 0), Quaternion.identity);
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
        Clear();
        EndScript end = EndScript.Instance;
        if (!debugThisSceneMode)
        {
            MainManager.Instance.Money += reward;
            MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);
        }

        end.gameObject.SetActive(true);
        end.btn.gameObject.GetComponentInChildren<Text>().text = "Вы сделали это!";
        end.btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

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
                    item.Sucks();
                    teacher.TeacherAngry();                  
                }         
            }
        }
    }
}
