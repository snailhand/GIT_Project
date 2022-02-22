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
                player.TakeDamage(damage);
            }
        }
    }
}
