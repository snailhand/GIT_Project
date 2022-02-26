using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    public float respawnDelay;
    public AmongUs player;
    public Vector3 lastCheckPointPos;
    private static GameMasterScript instance;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<AmongUs>();
        player.OnDeath.AddListener(OnPlayerDeath);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator StartRespawning()
    {
        var controller = player.controller;

        yield return new WaitForSeconds(respawnDelay);


        player.Respawn();
    }

    private void OnPlayerDeath()
    {
        StopAllCoroutines();
        StartCoroutine(StartRespawning());
    }
}
