using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerTapScript : MonoBehaviour
{
    public static ManagerTapScript Instance { get; private set; }
    // Start is called before the first frame update

    public TapScript tap;
    public delegate void OnTapSpeedChange();
    public event OnTapSpeedChange OnTapSpeedChangeHandler;


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


    public float endAnimationTime;

    private void Awake()
    {
        Instance = this;
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
    void UITheEndWon()
    {
        TheEndScript.Instance.gameObject.SetActive(true);

        TheEndScript.Instance.btn.gameObject.GetComponentInChildren<Text>().text = "You won!";

        TheEndScript.Instance.btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    void UITheEndLose()
    {
        TheEndScript.Instance.gameObject.SetActive(true);

        TheEndScript.Instance.btn.gameObject.GetComponentInChildren<Text>().text = "You lost!";

        TheEndScript.Instance.btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
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
        //if (tap.curLoad <= TapScript.minLoad && !tap.isEnding)
        //{
            
           
        //    Debug.Log("ending");
        //}
        //else if (tap.isEnding && timerAnimation >= 0)
        //{
        //    timerAnimation -= Time.deltaTime;
        //}
        //else if (tap.isEnding && timerAnimation < 0)
        //{
        //    OnTheEndHandler();
        //}
    }

    private void UIOnProgressChange()
    {
        //TODO
        //make update for progressbarhere
        //and somth else if y think we need
    }

   
}
