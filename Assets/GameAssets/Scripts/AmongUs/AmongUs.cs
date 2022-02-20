using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmongUs : Player<AmongUs>
{
    public UnityEvent OnJump;

    public UnityEvent OnHurt;

    public UnityEvent OnDeath;

    private Vector3 respawnPosition;
    private Quaternion respawnRoatation;

    public Player_InputManager inputs { get; private set; }

    public AmongUs_StatsManager stats { get; private set; }

    public Health health { get; private set; }

    public bool onWater { get; private set; }

    public int jumpCounter { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        inputs = GetComponent<Player_InputManager>();
        stats = GetComponent<AmongUs_StatsManager>();
        health = GetComponent<Health>();
        tag = "Player";

        OnGroundEnter.AddListener(ResetJumps);
    }

    public virtual void Respawn()
    {
        health.ResetHp();
        transform.SetPositionAndRotation(respawnPosition, respawnRoatation);
    }

    public virtual void SetRespawn(Vector3 position, Quaternion rotation)
    {
        respawnPosition = position;
        respawnRoatation = rotation;
    }

    public virtual void TakeDamage(int amount)
    {
        if(!health.isEmpty && !health.recovering)
        {
            health.Damage(amount);


            OnHurt?.Invoke();

            if(health.isEmpty)
            {
                OnDeath?.Invoke();
            }
        }
    }

    public virtual void Die()
    {
        health.Set(0);
        OnDeath?.Invoke();
    }

    public virtual void Accelerate(Vector3 direction)
    {
        Accelerate(direction, stats.current.turnDrag, stats.current.acceleration, stats.current.topSpeed);
    }

    public virtual void Decelerate()
    {
        Decelerate(stats.current.deceleration);
    }

    public virtual void Friction()
    {
        Decelerate(stats.current.friction);
    }

    public virtual void Gravity()
    {
        Gravity(stats.current.gravity);
    }

    public virtual void SnapToGround()
    {
        SnapToGround(stats.current.snapForce);
    }

    public virtual void FaceDirectionSmooth(Vector3 direction)
    {
        FaceDirection(direction, stats.current.rotationSpeed);
    }

    public virtual void Fall()
    {
        if(!isGrounded)
        {
            //states.Change<State_PlayerFall>();
        }
    }

    public virtual void Jump()
    {
        var canMultiJump = (jumpCounter > 0) && (jumpCounter < stats.current.multiJumps);
        var canBufferJump = (jumpCounter == 0) && (Time.time < lastGroundTime + stats.current.jumpBufferThreshold);

        if (isGrounded || canMultiJump || canBufferJump)
        {
            if (inputs.GetJumpDown())
            {
                Jump(stats.current.maxJumpHeight);
            }
        }

        if (inputs.GetJumpUp() && (jumpCounter > 0) && (verticalVelocity.y > stats.current.minJumpHeight))
        {
            verticalVelocity = Vector3.up * stats.current.minJumpHeight;
        }
    }

    public virtual void Jump(float height)
    {
        jumpCounter++;
        verticalVelocity = Vector3.up * stats.current.maxJumpHeight;
        OnJump?.Invoke();
    }

    public virtual void DirectionalJump(Vector3 direction, float height, float distance)
    {
        jumpCounter++;
        verticalVelocity = Vector3.up * height;
        horizontalVelocity = direction * distance;
        OnJump?.Invoke();
    }

    public virtual void ResetJumps() => jumpCounter = 0;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody)
        {
            var above = stepPosition.y > hit.collider.bounds.max.y;

            if (!above && CapsuleCast(hit.moveDirection, 0.25f))
            {
                var force = horizontalVelocity * stats.current.pushForce;
                hit.rigidbody.velocity += force / hit.rigidbody.mass * Time.deltaTime;
            }
        }

        if (hit.collider.CompareTag("Enemy"))
        {
            if ((velocity.y <= 0) && IsPointUnderStep(hit.point))
            {
                //Take damage
            }
            else
            {
                //Damage the enemy
            }
        }
        else if (!health.isEmpty)
        {

        }
    }


}
