using UnityEngine;

public class State_AmongUsWalk : AmongUsStates
{
	protected override void OnEnter(AmongUs player) { }

	protected override void OnExit(AmongUs player) { }

	protected override void OnStep(AmongUs player)
	{
		player.Gravity();
		player.SnapToGround();
		player.Jump();
		player.Fall();

		var inputDirection = player.inputs.GetCameraDirection(out var magnitude);

		if (magnitude > 0)
		{
			var dot = Vector3.Dot(inputDirection, player.horizontalVelocity);

			if (dot >= -0.8f)
			{
				player.Accelerate(inputDirection);
				player.FaceDirectionSmooth(player.horizontalVelocity);
			}
			else
			{
				//Stop state
			}
		}
		else
		{
			player.Friction();

			if (player.horizontalVelocity.sqrMagnitude <= 0)
			{
				//Idle State
			}
		}
	}

}
