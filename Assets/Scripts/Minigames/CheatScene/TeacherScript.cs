using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherScript : MonoBehaviour
{
    // time after last wathcing started
    public float time;

    // how much time left for watching
    public float timeWatching = 0;
    public float animationStartTime;
    public float animationStopTime;

    //time to cheat plus
    public float delta;
    public float movement;
    public float temp;

    public bool isTurning;
    public bool isAngry;

    //how much student could cheat 
    public float timeToCheat;
    public delegate void OnWatching();
    public event OnWatching IsWatchingHandler;

    public delegate void OnTurning();
    public event OnTurning IsStartWatchingHandler;

    public delegate void OnNotWatching();
    public event OnNotWatching IsNotWatchingHandler;

    void Start()
    {
        IsWatchingHandler += UIIsWatching;
        IsNotWatchingHandler += UIIsNotWatching;
        IsStartWatchingHandler += UIIsTurning;
        movement = 0.01f;

        //??? can we find it dynamically?
        animationStartTime = (float)2;
        animationStopTime = (float)2;
        isAngry = false;

        isTurning = false;
        ManagerScript.Instance.teacher = this;
        delta = 3;
        gameObject.transform.position = new Vector3(ManagerScript.Instance.cameraRect.x + ManagerScript.Instance.cameraRect.width / 2, ManagerScript.Instance.cameraRect.y + 5 * ManagerScript.Instance.cameraRect.height / 6, 0);
        //??? in oredr not to wait frequency time before first watching
        time = ManagerScript.FREQUENCY - 3;
    }

    // Update is called once per frame
    void Update()
    {        
        time += Time.deltaTime;
        if (isAngry)
        {
            return;
        }
        if (ManagerScript.ENOUGH * ManagerScript.Instance.studentAmount + delta <= timeToCheat)
        {
            ManagerScript.Instance.EndedTime();
        }
        if (ManagerScript.Instance.teacherWatching)
        {
            isTurning = false;
            timeWatching -= Time.deltaTime;
            if (timeWatching < 0)
            {
                IsNotWatchingHandler();
                ManagerScript.Instance.teacherWatching = false;
                timeWatching = 0;
            }
        }
        else if (isTurning)
        {
            if (time >= ManagerScript.FREQUENCY)
            {
                IsWatchingHandler();
                isTurning = false;
                timeToCheat += temp;
                temp = 0;
                ManagerScript.Instance.teacherWatching = true;
                time = 0;
                GenerateWatchingPeriod();
            }
        }
        else
        {
            if (time >= ManagerScript.FREQUENCY)
            {
                IsWatchingHandler();

                ManagerScript.Instance.teacherWatching = true;
                time = 0;
                GenerateWatchingPeriod();             
            }
            else if (time + animationStartTime >= ManagerScript.FREQUENCY)
            {
                //Debug.Log($"time minused. Time {time}");
                time = ManagerScript.FREQUENCY - animationStartTime;
                IsStartWatchingHandler();
                isTurning = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.transform.position.x < ManagerScript.Instance.cameraRect.x + ManagerScript.Instance.cameraRect.width / 4)
        {
            movement = -movement;
        }
        else if (gameObject.transform.position.x > ManagerScript.Instance.cameraRect.x + 3 * ManagerScript.Instance.cameraRect.width / 4)
        {
            movement = -movement;
        }
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + movement, gameObject.transform.position.y, 0);
    }

    void GenerateWatchingPeriod()
    {
        System.Random rnd = new System.Random();
        timeWatching = rnd.Next((ManagerScript.FREQUENCY - (int)(animationStopTime + animationStartTime))/3, (ManagerScript.FREQUENCY - (int)(animationStopTime + animationStartTime)) / 2);
        temp += ManagerScript.FREQUENCY - timeWatching;
    }

    void UIIsTurning()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1.0f);
        //TODO
        //place to start animation of turning teacher (if we have, else nothing)
    }

    void UIIsNotWatching()
    {
        Debug.Log("Busy!");
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        //TODO
        //if we have animation of turning teacher than put here start of it, else put a picture of not watching teacher
    }

    void UIIsWatching()
    {
        Debug.Log("Watching!");
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0f, 0f);
        //TODO
        //if we have no animation of turning teacher than put picture of teacher watching here
    }

    public void TeacherAngry()
    {
        isAngry = true;
        //TODO animation teacher is angry when saw cheating. Time less than animation of end
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
    }
}
