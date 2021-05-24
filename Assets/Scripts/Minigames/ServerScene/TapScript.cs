using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScript : MonoBehaviour
{

    public bool mouseDown;
    public float time;
    public float timeDown;
    public float tapSpeed;
    // public float newTapSpeed;
    public float newTapSpeed;
    public bool isEnding;
    public const float maxLoad = 100;
    public const float minLoad = 0;

    public float deltaDOWN;
    public float deltaUP;
    public float A;
    public float B;

    public float curLoad;

    public delegate void OnTapSpeedChanged();
    float acseleration;
    public delegate void OnStart();
    public event OnStart AnimationStartHandler;

    int downCounter;
    public int taps;
    public int times;
    float timing = 0;


    // Start is called before the first frame update

    void Start()
    {
        acseleration = 0.3f;
        mouseDown = false;
        timeDown = 0;
        isEnding = false;

        //params
        curLoad = 15;
        
        AnimationStartHandler += UIStartAnimation;
        AnimationStartHandler();
        //params
        A = 5;
        B = 6;
        deltaDOWN = (float)0.03;
        deltaUP = (float)0.04;

        ManagerTapScript.Instance.tap = this;
    }

    void UIStartAnimation()
    {
        //TODO or not
        //if you want to put some animation
        //again if you need some object reference just ask
    }

    void Update()
    {
        timing += Time.deltaTime;
        time += Time.deltaTime;

        
            if (Input.touchCount > 0)
            {
                print("ther is a touch");

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                downCounter++;
                    print("Touch has Began");
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    print("Touch has Ended");
                }
            }

        //newTapSpeed = 1 / (time - timeDown);

        if (newTapSpeed >= B)
        {
            if (newTapSpeed - B >= 0.5f * B)
            {
                curLoad += -(float)deltaDOWN * 3;
            }
            else if (newTapSpeed - B >= 0.33f * B)
            {
                curLoad += -(float)deltaDOWN * 2;
            }
            else
            {
                curLoad += -(float)deltaDOWN;
            }
        }

        if (newTapSpeed < A)
        {
            if (A - newTapSpeed >= 0.5f * A)
            {
                curLoad += (float)deltaUP * 3;
            }
            else if (A - newTapSpeed >= 0.33f * A)
            {
                curLoad += (float)deltaUP * 2;
            }
            else
            {
                curLoad += (float)deltaUP;
            }
        }

        tapSpeed = newTapSpeed;
        if (curLoad < minLoad)
        {
            curLoad = minLoad;
        }
        else if (curLoad > maxLoad)
        {
            curLoad = maxLoad;
        }
    }

    private void FixedUpdate()
    {
        if (timing >= acseleration)
        {
            timing = 0;
            newTapSpeed = downCounter / acseleration;
            downCounter = 0;
        }

    }

    void OnMouseDown()
    {
        if (!isEnding)
        {       
            mouseDown = true;
            timeDown = time;
            Debug.Log("dowwwwwn");
        }
    }

    /*void OnMouseUp()
    {
        if (mouseDown && !isEnding)
        {
            mouseDown = false;
        }
    }*/
}
