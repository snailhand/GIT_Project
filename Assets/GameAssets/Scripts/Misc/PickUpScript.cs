using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public bool rotate;
    public float speed;

    private float defaultYPosition;

    void Start()
    {
        defaultYPosition = this.transform.position.y;   
    }

    void Update()
    {
        if(rotate)
        {
            transform.Rotate(0, speed * 50 * Time.deltaTime, 0);
        }

        transform.position = new Vector3(transform.position.x, defaultYPosition + ((float)Mathf.Sin(Time.time) * speed), transform.position.z);
    }
}
