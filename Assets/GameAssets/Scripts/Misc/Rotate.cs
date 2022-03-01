using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float rotateSpeed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left * rotateSpeed * Time.deltaTime);
    }
}
