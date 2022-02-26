using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmongUs : Player<AmongUs>
{
    //If want to add sound/particle can use event//

    //Called when Player jumps 
    public UnityEvent OnJump;

    //Called when Player gets damaged
    public UnityEvent OnHurt;

    //Called when the Player dies
    public UnityEvent OnDeath;

    //Respawn Transforms
    private Vector3 respawnPosition;
    private Quaternion respawnRoatation;

    //Returns the Player Input Manager script instance
    public Player_InputManager inputs { get; private set; }

    //Returns the Player States Manager script instance
    public AmongUs_StatsManager stats { get; private set; }

    //Returns the Health script instance
    public Health health { get; private set; }

    //public bool onWater { get; private set; }

    //Returns how many times the Player has jumped
    public int jumpCounter { get; private set; }

    protected override void Awake()
    {
        //Reference components in transform
        base.Awake();
        inputs = GetComponent<Player_InputManager>();
        stats = GetComponent<AmongUs_StatsManager>();
        health = GetComponent<Health>();
        tag = "Player";

        OnGroundEnter.AddListener(ResetJumps);
    }

    private void LateUpdate()
    {
        //Lock x axis of player
        transform.position = new Vector3(3.43f, transform.position.y, transform.position.z);
    }

    //Resets player state, HP and transform
    public virtual void Respawn()
    {
        health.ResetHp();
        transform.SetPositionAndRotation(respawnPosition, respawnRoatation);
    }

    //Sets the position and rotation of the Player's respawn through checkpoints
    public virtual void SetRespawn(Vector3 position, Quaternion rotation)
    {
        respawnPosition = position;
        respawnRoatation = rotation;
    }

    //Decreases the Player's HP with proper interaction
    public virtual void TakeDamage(int amount, Vector3 distance)
    {
        if(!health.isEmpty && !health.recovering)
        {
            health.Damage(amount);
            //var minVelocity = stats.current.topSpeed / 2f;
            //var velocityLimit = horizontalVelocity.magnitude / minVelocity;

            //if(horizontalVelocity.magnitude > minVelocity && velocityLimit <= 2f)
            //{
            //    horizontalVelocity = distance * stats.current.hurtBackwards * velocityLimit;
            //}
            //else
            //{
            //    horizontalVelocity = distance * stats.current.hurtBackwards;
            //}

            //Applies force that pushes Player away from the obstacle
            horizontalVelocity = distance * stats.current.hurtBackwards;
            verticalVelocity = Vector3.up * stats.current.hurtUpwards;
            FaceDirection(-horizontalVelocity);

            //Changes current state to hurt state
            states.Change<State_AmongUsHurt>();
            OnHurt?.Invoke();

            if(health.isEmpty)
            {
                OnDeath?.Invoke();
            }
        }
    }

    //Kills the Player
    public virtual void Die()
    {
        health.Set(0);
        OnDeath?.Invoke();
    }

    //Moves the Player smoothly in a given direction
    public virtual void Accelerate(Vector3 direction)
    {
        Accelerate(direction, stats.current.turnDrag, stats.current.acceleration, stats.current.topSpeed);
    }

    //Slows the Player smoothly 
    public virtual void Decelerate()
    {
        Decelerate(stats.current.deceleration);
    }

    //Smoothly reduces the horizontal velocity of Player 
    public virtual void Friction()
    {
        Decelerate(stats.current.friction);
    }

    //Downwards force applied to the Player
    public virtual void Gravity()
    {
        Gravity(stats.current.gravity);
    }

    //Applies Downward force towards ground
    public virtual void SnapToGround()
    {
        SnapToGround(stats.current.snapForce);
    }

    //Rotates the player smoothly in a given direction
    public virtual void FaceDirectionSmooth(Vector3 direction)
    {
        FaceDirection(direction, stats.current.rotationSpeed);
    }

    //Transitions between Jump and Fall state
    public virtual void Fall()
    {
        if(!isGrounded)
        {
            states.Change<State_AmongUsFall>();
        }
    }

    //Handles jump from grounded state
    public virtual void Jump()
    {
        var canMultiJump = (jumpCounter > 0) && (jumpCounter < stats.current.multiJumps);
        var canBufferJump = (jumpCounter == 0) && (Time.time < lastGroundTime + stats.current.jumpBufferThreshold);

        if (isGrounded || canMultiJump || canBufferJump)
        {
            if (inputs.GetJumpDown())
            {
                //print("jump");
                Jump(stats.current.maxJumpHeight);
            }
        }
        //print("vertical velocity: " + verticalVelocity);

        if (inputs.GetJumpUp() && (jumpCounter > 0) && (verticalVelocity.y > stats.current.minJumpHeight))
        {
            verticalVelocity = Vector3.up * stats.current.minJumpHeight;
        }
    }

    //Applies an upward force to the Player
    public virtual void Jump(float height)
    {
        jumpCounter++;
        //print("jumpCounter: " + jumpCounter);
        verticalVelocity = Vector3.up * stats.current.maxJumpHeight;
        OnJump?.Invoke();
    }

    //Applies jump force to a given direction
    //public virtual void DirectionalJump(Vector3 direction, float height, float distance)
    //{
    //    jumpCounter++;
    //    verticalVelocity = Vector3.up * height;
    //    horizontalVelocity = direction * distance;
    //    OnJump?.Invoke();
    //}

    //Sets the jump counter back to 0 
    public virtual void ResetJumps()
    {
        jumpCounter = 0;
    }

    //Handles Collision through CharacterController
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
            //spring interaction
            if((hit.normal.y > 0.5f) && hit.collider.TryGetComponent<Spring>(out var spring))
            {
                verticalVelocity = Vector3.zero;
                spring.BounceUpwards(this);
            }

        }
    }


}
