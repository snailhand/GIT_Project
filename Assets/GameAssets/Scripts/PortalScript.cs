using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public bool spawnPlayer = true;
    public bool suckPlayer;

    private GameObject player;
    private bool sucking;
    public float delay = 4f;

    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if(spawnPlayer)
        {
            //Disable Player's controller
            controller.enabled = false;
            //Disable Player's mesh
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.transform.GetChild(1).gameObject.SetActive(false);

            StartCoroutine(SpawnInPlayer(controller, player));
        }
    }

    void Update()
    {
        MovePlayerIntoPortal();
    }

    private IEnumerator SpawnInPlayer(CharacterController controller, GameObject playerObj)
    {
        yield return new WaitForSeconds(delay);

        //Spawn in player
        controller.enabled = true;
        playerObj.transform.GetChild(0).gameObject.SetActive(true);
        playerObj.transform.GetChild(1).gameObject.SetActive(true);

        ClosePortal();
    }

    private IEnumerator SuckInPlayer(float time)
    {
        yield return new WaitForSeconds(time);

        //Disable Player's mesh
        player.transform.GetChild(0).gameObject.SetActive(false);
        player.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void MovePlayerIntoPortal()
    {
        var distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (suckPlayer && sucking && distanceFromPlayer >= 1)
        {
            //Suck in player
            player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, delay * Time.deltaTime);
        }
    }

    private void ClosePortal()
    {
        animator.Play("Close");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(suckPlayer && !spawnPlayer)
            {
                sucking = true;
                //Disable Player's controller
                controller.enabled = false;

                ClosePortal();

                StartCoroutine(SuckInPlayer(1.5f));
            }
        }
    }
}
