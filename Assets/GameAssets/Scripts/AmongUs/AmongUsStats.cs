using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "GameAssets/ScriptableObjects")]
public class AmongUsStats : PlayerStats<AmongUsStats>
{
    [Header("General Stats")]
    public float rotationSpeed = 600f;
    public float gravity = 50f;
    public float snapForce = 15f;
    public float pushForce = 4f;
    public float slideForce = 10f;

    [Header("Movement Stats")]
    public float acceleration = 20f;
    public float deceleration = 20f;
    public float friction = 15f;
    public float topSpeed = 5f;
    public float turnDrag = 30f;

    [Header("Jump Stats")]
    public int multiJumps = 1;
    public float jumpBufferThreshold = 0.15f;
    public float maxJumpHeight = 20f;
    public float minJumpHeight = 10f;

    [Header("Hurt Stats")]
    public float hurtUpwards = 10f;
    public float hurtBackwards = 10f;
}
