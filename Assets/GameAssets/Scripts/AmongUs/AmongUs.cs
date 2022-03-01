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

    //Called when Player spawns
    public UnityEvent OnSpawn;

    //Called when Player gets revived
    public UnityEvent OnRespawn;

    //Called when the Player dies
    public UnityEvent OnDeath;

    //Respawn Transforms
    [SerializeField]
    private Vector3 respawnPosition;
    private Quaternion respawnRotation;

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
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }

    //Resets player state, HP and transform
    public virtual void Respawn()
    {
        health.ResetHp();
        SetController(false);
        transform.SetPositionAndRotation(respawnPosition, respawnRotation);
        SetController(true);
        OnRespawn?.Invoke();

        states.Change<State_AmongUsIdle>();
    }

    //Sets the position and rotation of the Player's respawn through checkpoints
    public virtual void SetRespawn(Vector3 position, Quaternion rotation)
    {
        respawnPosition = position;
        respawnRotation = rotation;
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

    //Moves the Player down slopes
    protected override void HandleSlopeLimit(RaycastHit hit)
    {
        var slopeDirection = Vector3.Cross(hit.normal, Vector3.Cross(hit.normal, Vector3.up));
        slopeDirection = slopeDirection.normalized;
        controller.Move(slopeDirection * stats.current.slideForce * Time.deltaTime);
    }
    
    protected override void HandleHighLedge(RaycastHit hit)
    {
        var edgeNormal = hit.point - transform.position;
        var edgePushDirection = Vector3.Cross(edgeNormal, Vector3.Cross(edgeNormal, Vector3.up));
        controller.Move(edgePushDirection * stats.current.slideForce * Time.deltaTime);
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
            GameObject enemy;
            if (hit.collider.GetComponent<MeleeEnemyScript>())
            {
                enemy = hit.collider.GetComponent<MeleeEnemyScript>().gameObject;
            }
            else
            {
                enemy = hit.collider.GetComponent<RangedEnemyScript>().gameObject;
            }

            if ((velocity.y <= 0) && IsPointUnderStep(hit.point))
            {
                //Damage the enemy & bounce up
                var bounce = Mathf.Max(-verticalVelocity.y, stats.current.hurtUpwards);
                verticalVelocity = Vector3.up * bounce;

                //Trigger die for enemy
                if(enemy.GetComponent<MeleeEnemyScript>())
                {
                    enemy.GetComponent<MeleeEnemyScript>().Die();
                }
                else
                {
                    enemy.GetComponent<RangedEnemyScript>().Die();
                }
            }
            else
            {
                //Get direction of contact
                var collisionPoint = hit.collider.ClosestPoint(transform.position);
                Vector3 direction = (collisionPoint - transform.position).normalized;

                //Take damage
                TakeDamage(1, -direction);
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
