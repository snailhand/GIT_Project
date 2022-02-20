using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AmongUs))]
public class AmongUs_StateManager : Player_StateManager<AmongUs> 
{
    public List<AmongUsStates> states;

    protected override List<PlayerStates<AmongUs>> GetStateList()
    {
        return new List<PlayerStates<AmongUs>>(states);
    }

}
