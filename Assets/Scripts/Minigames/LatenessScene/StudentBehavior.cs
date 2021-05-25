using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StudentBehavior : MonoBehaviour, IDragHandler
{
    public Transform Student;
    private float minSwipe = 40;

    void Start()
    {
          
    }

    void Move(Vector3 direction){
        Student.GetComponent<Rigidbody2D>().AddForce(direction * 40f, ForceMode2D.Force);
    }

    public void OnDrag(PointerEventData eventData)
    {
       if (GameObject.Find("GameManager").GetComponent<Manager>().IsPlaying)
       {
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
            {
                if (eventData.delta.x > 0)
                {
                    Move(Vector3.right * Mathf.Abs(eventData.delta.x) / minSwipe);
                }
                else
                {
                    Move(Vector3.left * Mathf.Abs(eventData.delta.x) / minSwipe);
                }
            }
            else
            {
                if (eventData.delta.y > 0)
                {
                    Move(Vector3.up * Mathf.Abs(eventData.delta.x) / minSwipe * 5);
                }
                else
                {
                    Move(Vector3.down * Mathf.Abs(eventData.delta.x) / minSwipe * 5);
                }
            }
       }
       else
       {
           GameObject.Find("GameManager").SendMessage("StartGame");
       }
    }
}
