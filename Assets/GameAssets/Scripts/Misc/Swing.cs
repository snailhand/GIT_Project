using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public float speed;
    public float leftAngle;
    public float rightAngle;

    public bool movingClockwise;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
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

        if (movingClockwise)
        {
            rb.angularVelocity = Vector3.right * speed;
        }

        if (!movingClockwise)
        {
            rb.angularVelocity = Vector3.left * speed;
        }
    }
}
