using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMasterScript : MonoBehaviour
{
    public int score;
    public int hp;
    public int coins;

    public float respawnDelay;
    public AmongUs player;
    public Vector3 lastCheckPointPos;
    public static GameMasterScript instance;

    public UnityEvent OnHealthUpdated;
    public UnityEvent OnCoinsUpdated;

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
        yield return new WaitForSeconds(respawnDelay);
        player.Respawn();
    }

    private void OnPlayerDeath()
    {
        StopAllCoroutines();
        StartCoroutine(StartRespawning());
    }

    public void AddHP(int amount)
    {
        player.GetComponent<Health>().Increase(amount);
    }
}
