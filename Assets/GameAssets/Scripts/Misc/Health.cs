using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    //Starting Health
    public int startHp;
    //Max Health player can obtain
    public int maxHp;
    //Invulnerable period
    public float cooldown = 1f;

    public UnityEvent OnChange;

    private float timeSinceLastDamage;
    private int current;

    private void Awake()
    {
        ResetHp();
    }

    public int currentHp
    {
        get { return current; }

        private set
        {
            var last = current;
            
            if(value != last)
            {
                current = value;
                OnChange?.Invoke();
            }
        }
    }

    public bool isEmpty
    {
        get { return current == 0; }
    }

    public virtual bool recovering
    {
        get { return Time.time < timeSinceLastDamage + cooldown; }
    }

    public virtual void Set(int amount)
    {
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
    }

    public virtual void Increase(int amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHp);
    }

    public virtual void Damage(int amount)
    {
        if(!recovering)
        {
            currentHp -= amount;
            currentHp = Mathf.Max(current, 0);
            timeSinceLastDamage = Time.time;
        }
    }

    public virtual void ResetHp()
    {
        currentHp = startHp;
    }
}
