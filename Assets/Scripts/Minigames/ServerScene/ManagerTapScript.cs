using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerTapScript : MonoBehaviour
{
    
    public int penalty;
    public int reward;
    public static ManagerTapScript Instance { get; private set; }
    // Start is called before the first frame update

    public TapScript tap;
    public delegate void OnTapSpeedChange();
    public event OnTapSpeedChange OnTapSpeedChangeHandler;

    public GameObject background;
    //limits time
    public float timeForGame;

    public float scoresNeededToWin;

    public bool isWinning;

    //time for the end animation
    public float timerAnimation;

    public delegate void OnWinning();
    public event OnWinning OnWinningHandler;

    public delegate void OnLoosing();
    public event OnWinning OnLoosingHandler;

    public delegate void OnTheEnd();
    public event OnTheEnd OnTheEndWonHandler;
    public event OnTheEnd OnTheEndLoseHandler;

    public bool IsPlaying = false;
    public GameObject HintBox;
    public GameObject BackPanel;
    public GameObject EndButton;

    public float endAnimationTime;

    private bool isClicked = false;

    public void StartGame()
    {
        IsPlaying = true;
        HintBox.SetActive(false);
        BackPanel.GetComponent<UnityEngine.UI.Image>().color = Color.clear;
    }

    private void Awake()
    {
        Instance = this;


        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector2 scale = transform.localScale;
        scale.x *= cameraSize.x / spriteSize.x;
        scale.y *= cameraSize.y / spriteSize.y;
        background.transform.position = Vector2.zero; // Optional
        background.transform.localScale = scale;
    }

    void Start()
    {
        OnTapSpeedChangeHandler += UIOnProgressChange;
        OnWinningHandler += UIEndWinAnimation;

        OnLoosingHandler += UIEndLooseAnimation;
        OnTheEndWonHandler += UITheEndWon;
        OnTheEndLoseHandler += UITheEndLose;
        //params
        endAnimationTime = 2;
        timerAnimation = endAnimationTime;

        //params
        timeForGame = 10;

        //params
        scoresNeededToWin = 5;

        isWinning = false;
    }

    public void FinishGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    void UITheEndWon()
    {
        MainManager.Instance.Money += reward;
        MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);
        EndButton.SetActive(true);

        EndButton.GetComponentInChildren<Text>().text = "You won!";
    }

    void UITheEndLose()
    {
        MainManager.Instance.Money -= penalty;
        MainManager.Instance.SetSceneCompleted(gameObject.scene.name, true);
        
        EndButton.SetActive(true);
        EndButton.GetComponentInChildren<Text>().text = "You lost!";
        //EndButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            SceneManager.LoadScene("MainScene");
        }
    }

    void UIEndWinAnimation()
    {
        //TODO
        //put last animation about winning
    }

    void UIEndLooseAnimation()
    {
        //TODO
        //put last animation about loosing
    }

    void EndingBackend()
    {
       // tap.time = endAnimationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
            if (!tap.isEnding)
            {
                GetComponentInChildren<Canvas>().gameObject.GetComponentInChildren<Text>().text = tap.curLoad.ToString();
            }
            if (tap.time >= timeForGame && !tap.isEnding)
            {
                tap.isEnding = true;
                if (tap.curLoad >= scoresNeededToWin)
                {
                    //loose
                    OnLoosingHandler();
                    
                }
                else
                {
                    //win
                    OnWinningHandler();
                    isWinning = true;
                }
                
            }
            else if (tap.isEnding)
            {
                if (timerAnimation > 0)
                {
                    timerAnimation -= Time.deltaTime;
                }
                else
                {
                    if (isWinning)
                    {
                        OnTheEndWonHandler();
                    }
                    else
                    {
                        OnTheEndLoseHandler();
                    }
                }
            }
        }
        
    }

    private void UIOnProgressChange()
    {
        //TODO
        //make update for progressbarhere
        //and somth else if y think we need
    }

   
}
