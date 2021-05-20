using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupervisorBehavior : MonoBehaviour
{

    public Transform Supervisor;
    public Transform Student;
    public float WalkSpeed = 0.25f;
    public float RotationSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void IncreaseSpeed()
    {
        WalkSpeed *= 1.6f;
    }

    // Update is called once per frame
    void Update()
    {
        // Really SHIT CODE for rotation, sorry pls

        float angle = Mathf.Atan2(Supervisor.position.y - Student.position.y, Supervisor.position.x - Student.position.x) * Mathf.Rad2Deg;
        var TargetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));
        Supervisor.rotation = Quaternion.Lerp(Supervisor.rotation, TargetRotation, Time.deltaTime * RotationSpeed);

        Supervisor.position = Vector3.MoveTowards(Supervisor.position, Student.position, Time.deltaTime * WalkSpeed);
    }
}
