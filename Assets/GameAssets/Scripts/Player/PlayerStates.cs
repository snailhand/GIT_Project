using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerStates<T> : MonoBehaviour where T : AmongUs<T>
{
    public UnityEvent onEnter;
    public UnityEvent onExit;

    public void Enter(T player)
    {
        onEnter?.Invoke();
        OnEnter(player);
    }

    public void Exit(T player)
    {
        onExit?.Invoke();
        OnExit(player);
    }

    public void Step(T player)
    {
        OnStep(player);
    }

    //Caleld when this state is invoked
    protected abstract void OnEnter(T player);

    //Called when thsi state changes to another
    protected abstract void OnExit(T player);

    //Called every frame where this state is activated
    protected abstract void OnStep(T player);
}
