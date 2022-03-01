    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyScript : MonoBehaviour
{
    private string currentState = "IdleState";
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Animator animator;

    public float chaseRange;
    public float speed;
    public float attackRange;

    public AudioClip[] die;
    private AudioSource audioSource;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        float distance = Vector3.Distance(transform.position, target.position);

        if (currentState == "IdleState")
        {
            animator.Play("Idle");
            if (distance < chaseRange)
            {
                currentState = "ChaseState";
            }
        }
        else if (currentState == "ChaseState")
        {
            animator.Play("Chase");

            if (distance < attackRange)
            {
                currentState = "AttackState";
            }

            if (target.position.z < transform.position.z)
            {
                transform.Translate(-transform.forward * speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.Translate(transform.forward * speed * Time.deltaTime);
                transform.rotation = Quaternion.identity;
            }
        }
        else if (currentState == "AttackState")
        {
            animator.Play("Idle");
            if (distance > attackRange)
            {
                currentState = "ChaseState";
            }
        }
    }

    public void Die()
    {
        audioSource.PlayOneShot(die[Random.Range(0, die.Length)]);
        Destroy(gameObject, 0.1f);
    }
}