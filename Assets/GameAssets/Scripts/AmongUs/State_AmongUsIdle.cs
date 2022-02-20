using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AmongUsIdle : AmongUsStates
{
	protected override void OnEnter(AmongUs player) { }

	protected override void OnExit(AmongUs player) { }

	protected override void OnStep(AmongUs player)
	{
		player.Gravity();
		player.SnapToGround();
		player.Jump();
		player.Fall();

		player.inputs.GetFacingDirection(out var magnitude);

		if (magnitude > 0 || player.horizontalVelocity.sqrMagnitude > 0)
		{
			player.states.Change<State_AmongUsWalk>();
		}
	}
}
