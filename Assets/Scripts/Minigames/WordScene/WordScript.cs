using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordScript : MonoBehaviour
{
    public bool mouseDown;
    Rect cameraRect;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start:  ");
        mouseDown = false;
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
       // ManagerWordsScript.Instance.cameraRect = cameraRect;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && mouseDown)
        {
            Vector3 pos = Input.GetTouch(0).position;
            pos = Camera.main.ScreenToWorldPoint(pos);
            //distance = pos.x - touchPos.x;
            float x = pos.x;
            if (pos.x < cameraRect.x)
            {
                x = cameraRect.x;
            }
            else if (pos.x > cameraRect.x + cameraRect.width)
            {
                x = cameraRect.x + cameraRect.width;
            }
            float y = pos.y;
            if (pos.y < cameraRect.y)
            {
                y = cameraRect.y;
            }
            else if (pos.y > cameraRect.y + cameraRect.height/2)
            {
                y = cameraRect.y + cameraRect.height/2;
            }

            transform.position = new Vector3(x, y, 0);
     
        }
        else
        {
            Vector3 pos = gameObject.transform.position;
            float x = pos.x;
            if (pos.x < cameraRect.x)
            {
                x = cameraRect.x;
            }
            else if (pos.x > cameraRect.x + cameraRect.width)
            {
                x = cameraRect.x + cameraRect.width;
            }
            float y = pos.y;
            if (pos.y < cameraRect.y)
            {
                y = cameraRect.y;
            }
            else if (pos.y > cameraRect.y + cameraRect.height / 2)
            {
                y = cameraRect.y + cameraRect.height / 2;
            }

            transform.position = new Vector3(x, y, 0);
        }
    }

    void OnMouseDown()
    {
        mouseDown = true;
        ManagerWordsScript.Instance.mouseDown = true;
    }
    void OnMouseUp()
    {
        mouseDown = false;
        ManagerWordsScript.Instance.mouseDown = false;
    }   
}
