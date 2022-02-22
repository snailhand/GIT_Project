using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    Rigidbody rb;

    public float currentSpeed;
    public float maxSpeed;
    public float minSpeed;

    public float leftAngle;
    public float rightAngle;

    bool movingClockwise;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movingClockwise = true;
    }

    void Update()
    {
        Move();
    }

    private void ChangeMoveDir()
    {
        if (movingClockwise && transform.rotation.x > rightAngle)
        {
            movingClockwise = false;

        }
        if (!movingClockwise && transform.rotation.x < leftAngle)
        {
            movingClockwise = true;

        }

    }

    private void Move()
    {
        ChangeMoveDir();

        if(currentSpeed == maxSpeed)
        {
            if (movingClockwise)
            {
                rb.angularVelocity = Vector3.right * currentSpeed;
            }

            if (!movingClockwise)
            {
                rb.angularVelocity = Vector3.left * currentSpeed;
            }

        }
    }
}
