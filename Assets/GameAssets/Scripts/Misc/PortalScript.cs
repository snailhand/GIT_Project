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

    private AudioSource _audio;
    public AudioClip portalOpen;
    public AudioClip portalClose;

    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

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
        StartCoroutine(PlaySound(1.1f, portalOpen));
        yield return new WaitForSeconds(delay);

        //Spawn in player
        controller.enabled = true;
        playerObj.transform.GetChild(0).gameObject.SetActive(true);
        playerObj.transform.GetChild(1).gameObject.SetActive(true);
        playerObj.GetComponent<AmongUs>().OnSpawn?.Invoke();

        ClosePortal();
    }

    private IEnumerator SuckInPlayer(float time)
    {
        yield return new WaitForSeconds(time);

        //Disable Player's mesh
        player.transform.GetChild(0).gameObject.SetActive(false);
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.GetComponent<AmongUs>().OnSpawn?.Invoke();

        StartCoroutine(NextLevel());
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
        StartCoroutine(PlaySound(2, portalClose));
        animator.Play("Close");
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(delay/2);

        SceneHandler.instance.NextLevel();
    }

    private IEnumerator PlaySound(float delay, AudioClip sound)
    {
        yield return new WaitForSeconds(delay);

        _audio.PlayOneShot(sound);
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
