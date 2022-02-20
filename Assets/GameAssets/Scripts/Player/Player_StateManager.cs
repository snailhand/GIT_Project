using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_StateManager<T> : MonoBehaviour where T : AmongUs<T>
{
    private List<PlayerStates<T>> m_list = new List<PlayerStates<T>>();

    private Dictionary<Type, PlayerStates<T>> m_states = new Dictionary<Type, PlayerStates<T>>();

    //Returns the instance of the current Player state
    public PlayerStates<T> current { get; private set; }

    //Returns the index of the current Plary state
    public int index()
    {
        return m_list.IndexOf(current);
    }

    //Returns the instance of the Player related to this state manager
    public T player { get; private set; }

    protected abstract List<PlayerStates<T>> GetStateList();

    //Initializes Player & Player states on start
    protected virtual void Start()
    {
        InitializePlayer();
        InitializeStates();
    }

    protected virtual void InitializePlayer()
    {
        player = GetComponent<T>();
    }

    protected virtual void InitializeStates()
    {
        m_list = GetStateList();

        foreach (var state in m_list)
        {
            var type = state.GetType();

            if(!m_states.ContainsKey(type))
            {
                m_states.Add(type, state);
            }
        }

        if(m_list.Count > 0)
        {
            current = m_list[0];
        }
    }

    //Change to a given Player state based on its INDEX on the States List.
    public virtual void Change(int toState)
    {
        if(toState >= 0 && toState < m_list.Count)
        {
            Change(m_list[toState]);
        }
    }

    //Change to a given Player state based on its CLASS TYPE.
    public virtual void Change<TState>() where TState : PlayerStates<T>
    {
        var type = typeof(TState);

        if (m_states.ContainsKey(type))
        {
            Change(m_states[type]);
        }
    }

    //Change to a given Player state based on its INSTANCE.
    public virtual void Change(PlayerStates<T> toState)
    {
        if(toState)
        {
            if(current)
            {
                current.Exit(player);
            }

            current = toState;
            current.Enter(player);
        }
    }

    public virtual void Step()
    {
        if(current)
        {
            current.Step(player);
        }
    }
}

