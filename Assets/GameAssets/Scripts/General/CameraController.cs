using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;      

public class CameraController : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;

    public float smoothSpeed = 0.15f;

    private void Start()
    {
        AssignCameraToPlayer(GameObject.FindGameObjectWithTag("Player").transform);
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    public void AssignCameraToPlayer(Transform player)
    {
        target = player;
        offset = transform.position - target.position;
    }
}
