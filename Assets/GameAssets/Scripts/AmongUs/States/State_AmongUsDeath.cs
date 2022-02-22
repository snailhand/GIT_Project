using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AmongUsDeath : AmongUsStates
{
	protected override void OnEnter(AmongUs player) { }

	protected override void OnExit(AmongUs player) { }

	protected override void OnStep(AmongUs player)
	{
		player.Gravity();
		player.Friction();
		player.SnapToGround();

		Debug.Log("State: Dead");
	}
}

