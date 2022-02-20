using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Player<T> : MonoBehaviour where T : Player<T>
{
    //Called when the Player lands on the ground
    public UnityEvent OnGroundEnter;

    //Called when the Player leaves the ground
    public UnityEvent OnGroundExit;

    //Collider detection for contacts
    private Collider[] contacts = new Collider[5];

    //Returns the state manager of the Player
    public Player_StateManager<T> states { get; private set; }

    //Returns the Character Controller of the Player
    public CharacterController controller { get; private set; }

    //Vector3 of the current velocity of the Player
    public Vector3 velocity { get; set; }

    //The current XZ velocity of the Player
    public Vector3 horizontalVelocity
    {
        get { return new Vector3(velocity.x, 0, velocity.z); }
        set { velocity = new Vector3(value.x, velocity.y, value.z); }
    }

    //The current Y velocity of the Player
    public Vector3 verticalVelocity
    {
        get { return new Vector3(0, velocity.y, 0); }
        set { velocity = new Vector3(velocity.x, value.y, velocity.z); }
    }

    //Returns the bottom position of the Player considering the step Offset
    public Vector3 stepPosition
    {
        get { return transform.position - transform.up * (height * 0.5f - controller.stepOffset); }
    }

    // Returns the last frame the Player was grounded
    public float lastGroundTime { get; private set; }

    // Returns true if the Player is on the ground.
    public bool isGrounded { get; private set; } = true;

    // Returns the collider height of the Player.
    public float height
    {
        get { return controller.height; } 
    }

    // Returns the collider radius of the Player.
    public float radius
    {
        get { return controller.radius; }
    }

    protected virtual void Awake()
    {
        InitializeController();
        InitializeStatesManager();
    }

    protected virtual void Update()
    {
        if(controller.enabled)
        {
            HandleStates();
            HandleController();
            HandleGround();
            HandleCeiling();
            OnUpdate();
        }
    }

    //Get CharacterController from Player transform
    protected virtual void InitializeController()
    {
        controller = GetComponent<CharacterController>();

        if(!controller)
        {
            //Create controller if controller == null
            controller = gameObject.AddComponent<CharacterController>();
            controller.skinWidth = 0.005f;
            controller.minMoveDistance = 0;
        }
    }

    //Get StateManager script from Player transform
    protected virtual void InitializeStatesManager()
    {
        states = GetComponent<Player_StateManager<T>>();
    }

    public virtual bool CapsuleCast(Vector3 direction, float distance, int layer = Physics.DefaultRaycastLayers,
    QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
    {
        return CapsuleCast(direction, distance, out _, layer, queryTriggerInteraction);
    }

    public virtual bool CapsuleCast(Vector3 direction, float distance,
        out RaycastHit hit, int layer = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
    {
        var offset = transform.up * (height * 0.5f - radius);
        var top = transform.position + offset;
        var bottom = transform.position - offset;
        return Physics.CapsuleCast(top, bottom, radius, direction,
            out hit, distance, layer, queryTriggerInteraction);
    }

    //Returns true if a point is below the Player's step position
    public virtual bool IsPointUnderStep(Vector3 point)
    {
        return (stepPosition.y > point.y);
    }

    //Calls Step method in State manager
    protected virtual void HandleStates()
    {
        if(states != null)
            states.Step();

    }

    //Calls CharacterController Move method
    protected virtual void HandleController()
    {
        controller.Move(velocity * Time.deltaTime);
    }

    //Using Raycast to check for ground contact
    protected virtual void HandleGround()
    {
        var distance = (height * 0.5f) - controller.radius + 0.1f;

        if (Physics.SphereCast(transform.position, radius, -transform.up, out var hit, distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            //If previous state is Falling or Landing
            if (!isGrounded && EvaluateLanding(hit) && (verticalVelocity.y <= 0))
            {
                isGrounded = true;
                OnGroundEnter?.Invoke();
            }

            //If previous state is already in contact with ground
            if (isGrounded)
            {
                if (IsPointUnderStep(hit.point))
                {
                    //Parent player's transform onto platform
                    if (hit.collider.CompareTag("Platform"))
                        transform.parent = hit.transform;
                    else
                        transform.parent = null;

                    if (Physics.Raycast(transform.position, -transform.up, out var hit1, height, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                    {
                        if (Vector3.Angle(hit1.normal, Vector3.up) >= controller.slopeLimit)
                        {
                            HandleSlopeLimit(hit1);
                        }
                    }
                }
                else
                {
                    HandleHighLedge(hit);
                }
            }
        }
        else if(isGrounded)
        {
            isGrounded = false;
            transform.parent = null;

            lastGroundTime = Time.time;
            verticalVelocity = Vector3.Max(verticalVelocity, Vector3.zero);

            OnGroundExit?.Invoke();
        }
    }

    //Using Raycasts to check for Ceiling contact
    protected virtual void HandleCeiling()
    {
        var distance = (height * 0.5f) - controller.radius + 0.1f;

        if (Physics.SphereCast(transform.position, radius, transform.up, out var hit, distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (verticalVelocity.y > 0)
            {
                verticalVelocity = Vector3.zero;
            }

            HandleCeilingCollision(hit);
        }
    }

    protected virtual bool EvaluateLanding(RaycastHit hit)
    {
        return true;
    }

    protected virtual void HandleSlopeLimit(RaycastHit hit) { }

    protected virtual void HandleHighLedge(RaycastHit hit) { }

    protected virtual void HandleCeilingCollision(RaycastHit hit) { }

    protected virtual void OnUpdate() { }

    //Moves the Player smoothly in a given direction with velocity
    public virtual void Accelerate(Vector3 direction, float turnDrag, float acceleration, float topSpeed)
    {
        if(direction.sqrMagnitude > 0)
        {
            var speed = Vector3.Dot(direction, horizontalVelocity);
            var velocity = direction * speed;

            var turningVelocity = horizontalVelocity - velocity;
            var turningDuration = turnDrag * Time.deltaTime;

            speed += acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, -topSpeed, topSpeed);

            velocity = direction * speed;
            turningVelocity = Vector3.MoveTowards(turningVelocity, Vector3.zero, turningDuration);
            horizontalVelocity = velocity + turningVelocity;
        }
    }

    //Smoothly decreases the Player's HORIZONTAL velocity to zero
    public virtual void Decelerate(float deceleration)
    {
        var time = deceleration * Time.deltaTime;
        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, time);
    }

    //Smoothly decreases the Player's VERTICAL velocity to zero
    public virtual void Gravity(float gravity)
    {
        if(!isGrounded)
        {
            verticalVelocity += Vector3.down * gravity * Time.deltaTime;
        }
    }

    //Applies force towards the ground when grounded (simulating gravity)
    public virtual void SnapToGround(float force)
    {
        if(isGrounded && (verticalVelocity.y <= 0))
        {
            verticalVelocity = Vector3.down * force;
        }
    }

    //Rotates the Player towards the given dirctioin
    public virtual void FaceDirection(Vector3 direction)
    {
        if(direction.sqrMagnitude > 0)
        {
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
        }
    }

    //Rotates the Player towards a given direction OVER TIME
    public virtual void FaceDirection(Vector3 direction, float degreesPerSecond)
    {
        if (direction.sqrMagnitude > 0)
        {
            var rotation = transform.rotation;
            var rotationDelta = degreesPerSecond * Time.deltaTime;
            var target = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(rotation, target, rotationDelta);
        }
    }

}
