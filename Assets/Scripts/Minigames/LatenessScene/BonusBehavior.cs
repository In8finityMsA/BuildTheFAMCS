using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBehavior : MonoBehaviour
{
    public GameObject Supervisor;
    public bool isBaff;

    // Start is called before the first frame update
    void Start()
    {
        Supervisor = GameObject.Find("Supervisor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBaff)
        {
            GameObject.Find("GameManager").SendMessage("StepForWin");
            Destroy(gameObject);
            Debug.Log("Good");
        }
        else
        {
            Supervisor.transform.localScale += new Vector3(1, 1, 0);
            Supervisor.SendMessage("IncreaseSpeed");
            Destroy(gameObject);
            Debug.Log("Bad");
        }
    }
}
