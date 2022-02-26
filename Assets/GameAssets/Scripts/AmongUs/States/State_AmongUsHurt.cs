using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AmongUsHurt : AmongUsStates
{
	protected override void OnEnter(AmongUs player) { }

	protected override void OnExit(AmongUs player) { }

	protected override void OnStep(AmongUs player)
	{
		player.Gravity();

		if (player.isGrounded && (player.verticalVelocity.y <= 0))
		{
			if (player.health.currentHp > 0)
			{
				player.states.Change<State_AmongUsIdle>();
			}
			else
			{
				player.states.Change<State_AmongUsDeath>();
			}
		}
	}
}
