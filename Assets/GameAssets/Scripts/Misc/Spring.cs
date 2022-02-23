﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float force;

    public void BounceUpwards(AmongUs player)
    {
        if(player.verticalVelocity.y <= 0)
        {
            player.verticalVelocity = Vector3.up * force;
        }
    }

    private void PushInDirection(Vector3 direction, AmongUs player)
    {
        player.horizontalVelocity = direction * force;
        player.verticalVelocity = Vector3.up * force;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<AmongUs>(out var player))
            {
                //Get direction of contact
                var collisionPoint = other.ClosestPoint(transform.position);
                Vector3 direction = (collisionPoint - transform.position).normalized;

                PushInDirection(direction, player);
            }
        }
    }
}
