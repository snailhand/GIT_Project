using UnityEngine;

public abstract class Player_StatsManager<T> : MonoBehaviour where T : PlayerStats<T>
{
    public T[] stats;

    public T current { get; private set; }

    public virtual void Change(int to)
    {
        if(to >= 0 && to < stats.Length)
        {
            if(current != stats[to])
            {
                current = stats[to];
            }
        }
    }

    void Start()
    {
        if(stats.Length > 0)
        {
            current = stats[0];
        }
    }
}
