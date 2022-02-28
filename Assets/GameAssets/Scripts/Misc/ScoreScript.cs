using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    private GameMasterScript gm;

    void Start()
    {
        gm = GameMasterScript.instance;
    }

    public void AddCoins(int amount)
    {
        gm.coins += amount;
        gm.OnCoinsUpdated?.Invoke();
    }

    public void AddHealth(int amount)
    {
        gm.hp += amount;
        gm.AddHP(amount);
        gm.OnHealthUpdated?.Invoke();        
    }
}
