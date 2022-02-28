using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private bool hit;
    public Transform target;
    public float speed;

    void Update()
    {
        if(!hit)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
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

                transform.localScale = new Vector3(2, 2, 2);
                hit = true;

                Destroy(gameObject,.3f);
                player.TakeDamage(1, direction);
            }
        }
    }
}
