using UnityEngine;

public class State_AmongUsBrake : AmongUsStates
{
	protected override void OnEnter(AmongUs player) { }

	protected override void OnExit(AmongUs player) { }

	protected override void OnStep(AmongUs player)
	{
		player.Gravity();
		player.SnapToGround();
		player.Jump();
		player.Fall();
		player.Decelerate();

		if (player.horizontalVelocity.sqrMagnitude == 0)
		{
			player.states.Change<State_AmongUsIdle>();
		}
	}

}
