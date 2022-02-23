using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int damage;

    private Collider obstacleCollider;

    private void Awake()
    {
        obstacleCollider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.TryGetComponent<AmongUs>(out var player))
            {
                //Get direction of contact
                var collisionPoint = other.ClosestPoint(transform.position);
                Vector3 direction = (collisionPoint - transform.position).normalized;

                player.TakeDamage(damage, direction);
            }
        }
    }
}
