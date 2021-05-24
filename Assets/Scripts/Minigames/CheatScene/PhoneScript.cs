using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PhoneScript : MonoBehaviour
{
    public bool mouseDown = false;
    public Animator animator;
    public bool IsCheating = false;

    //progress for this object (pupil)
    public float progress;
    public bool isCheating;
    public bool cheated;

    //you have died and now watch animation
    public bool isEnding;

    public float timerOfTheEnd;

    public float animationTime;

    private delegate void OnCheat();
    private delegate void OnWaitToCheat();
    private delegate void OnCheated();

    private event OnCheat OnCheatHandler;
    private event OnWaitToCheat OnWaitToCheatHandler;
    private event OnCheated OnCheatedHandler;

    private delegate void OnEndAnimation();
    private event OnEndAnimation AnimationOfEnd;

    


    void Start()
    {
        OnCheatHandler += BackendCheating;
        OnCheatHandler += UICheating;

        OnWaitToCheatHandler += BackendWaitToCheat;
        OnWaitToCheatHandler += UIWaitToCheat;

        OnCheatedHandler += Cheated;

        AnimationOfEnd += Ending;
        AnimationOfEnd += BackendEnding;



        isCheating = false;
        animator.SetBool("IsCheating", false);
        cheated = false;
        isEnding = false;
        progress = 0;
        timerOfTheEnd = 0;

        //???
        animationTime = 2;

        ManagerScript.Instance.students.Add(this);
    }

    void Update()
    {
        if (!isEnding)
        {
            if (progress >= ManagerScript.ENOUGH && !cheated)
            {
                cheated = true;
                isCheating = false;
                animator.SetBool("IsCheating", false);
                OnCheatedHandler();
            }
            if (mouseDown && !cheated)
            {
                ManagerScript.Instance.CurrentProgress += Time.deltaTime;
                progress += Time.deltaTime;
            }          
        }
        else
        {
            timerOfTheEnd -= Time.deltaTime;
            if (timerOfTheEnd <= 0)
            {
                ManagerScript.Instance.EndHappened();
            }
        }       
    }

    public void Cheated()
    {
        //TODO:
        //show that student has cheated
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 1.0f);
    }

    public void Sucks()
    {
        //TODO:
        //show somehow that person was caught by teacher
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.8f);

        //gameObject.SetActive(false);
        if (!isEnding)
        {
            AnimationOfEnd();
        }           
    }

    public void Ending()
    {
        //TODO start animation of end
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
    }

    public void BackendEnding()
    {
        isEnding = true;
        timerOfTheEnd = animationTime;
    }
    
    
   

    void BackendCheating()
    {
        isCheating = true;
        animator.SetBool("IsCheating", true);
    }

    void BackendWaitToCheat()
    {
        isCheating = false;
        animator.SetBool("IsCheating", false);
    }

    void UICheating()
    {
        //TODO:
        //show somehow that person is cheating
        animator.SetBool("IsCheating", true);
        GetComponent<SpriteRenderer>().color = new Color(0f, 1.0f, 0f);
    }

    void UIWaitToCheat()
    {
        //TODO:
        //show that person is not cheating
        animator.SetBool("IsCheating", false);
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
    }

    void OnMouseDown()
    {
        if (!mouseDown && !cheated && !isEnding)
        {
            //start of cheating animation
            mouseDown = true;
            OnCheatHandler();
            Debug.Log("cheating");
        }      
    }

    void OnMouseUp()
    {
        if (mouseDown && !cheated && !isEnding)
        {
            //start of waiting to cheat animation
            mouseDown = false;
            OnWaitToCheatHandler();
            Debug.Log("not cheating");
        }
    }
}
