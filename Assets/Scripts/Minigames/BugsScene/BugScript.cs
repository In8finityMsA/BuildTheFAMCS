using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float accelerationTime = 5f;
    public float maxSpeed = 10f;
    private Vector2 movement;
    private float timeLeft;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            timeLeft += accelerationTime;
        }
    }
    private void OnMouseDown()
    {
        ManagerBugsScript.Instance.bugs.Remove(this.gameObject);
        gameObject.SetActive(false);
    }
    private void OnMouseUp()
    {
       
    }

    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }
}
