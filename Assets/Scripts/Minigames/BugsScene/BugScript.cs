using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float accelerationTime = 0.1f;
    public float maxSpeed = 50f;
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
            movement = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
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
