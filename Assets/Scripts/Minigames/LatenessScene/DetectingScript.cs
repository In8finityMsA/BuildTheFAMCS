using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingScript : MonoBehaviour
{

    public float TimeToLose;
    private float time;
    private bool IsLose = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLose && time >= TimeToLose)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
            IsLose = true;
        }
        else if (IsLose)
        {
            GameObject.Find("GameManager").SendMessage("Lose");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsLose)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.92f, 0.016f, 1f);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        time += Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsLose)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
