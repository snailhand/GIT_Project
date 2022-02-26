using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyScript : MonoBehaviour
{
    private string currentState = "IdleState";
    private Transform target;
    public float chaseRange;
    public Animator animator;
    public float speed;
    public float attackRange;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (currentState == "IdleState")
        {
            if (distance < chaseRange)
            {
                currentState = "ChaseState");
            }
        }
        else if (currentState == "ChaseState")
        {
            animator.SetTrigger("ChaseAnimation");
            animator.SetBool("isAttacking", false);

            if (distance < attackRange)
            {
                currentState = "AttackState";
            }

            if (target.position.x > transform.position.x)
            {
                transform.Translate(transform.right * speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.Translate(-transform.right * speed * Time.deltaTime);
                transform.rotation = Quaternion.identity;
            }
        }
        else if (currentState == "AttackState")
        {
            animator.SetBool("isAttacking", true);
            if (distance > attackRange)
            {
                currentState = "ChaseState";
            }
        }
    }
}