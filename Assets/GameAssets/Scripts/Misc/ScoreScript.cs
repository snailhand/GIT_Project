using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    private GameMasterScript gm;
    private AudioSource audioSource;
    public AudioClip[] coinSFX;
    public AudioClip[] hpSFX;


    void Start()
    {
        gm = GameMasterScript.instance;
        audioSource = GetComponent<AudioSource>();
    }

    public void AddCoins(int amount)
    {
        audioSource.PlayOneShot(coinSFX[Random.Range(0, coinSFX.Length)]);
        gm.coins += amount;
        gm.OnCoinsUpdated?.Invoke();
    }

    public void AddHealth(int amount)
    {
        audioSource.PlayOneShot(hpSFX[Random.Range(0, hpSFX.Length)]);
        gm.hp += amount;
        gm.AddHP(amount);
        gm.OnHealthUpdated?.Invoke();        
    }
}
