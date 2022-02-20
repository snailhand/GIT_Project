using UnityEngine;

public class State_AmongUsFall : AmongUsStates
{
	protected override void OnEnter(AmongUs player) { }

	protected override void OnExit(AmongUs player) { }

	protected override void OnStep(AmongUs player)
	{
		player.Gravity();
		player.SnapToGround();
		player.Jump();

		print("State: Falling");
		if (!player.isGrounded)
		{
			var inputDirection = player.inputs.GetCameraDirection(out var magnitude);

			if (magnitude > 0)
			{
				var dot = Vector3.Dot(inputDirection, player.horizontalVelocity);

				if (dot >= 0)
				{
					player.Accelerate(inputDirection);
					player.FaceDirectionSmooth(player.horizontalVelocity);
				}
				else
				{
					player.Decelerate();
				}
			}
		}
		else
		{
			player.states.Change<State_AmongUsIdle>();
		}
	}
}
