using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using static System.TimeZoneInfo;
using System.ComponentModel;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("Jump variables")]
    public bool isGrounded;
    [SerializeField]private bool grappleGravity;


    [SerializeField] private bool pressingJump = false;
    private bool midJump = false;
    public bool falling = false;
    public bool slowfalling = false;

    public float maxJumpDuration = 0.5f;

    [Header("Slope Variables")]
    public bool onSlope = false;
    public bool walkableSlope = true;
    public Vector2 slopeDirection;

    [Header("Gravity")]
    public float gravityUpwards = 4;
    public float downwardGravity = 10;
    public AnimationCurve jumpGravityTransitionSpeed;
    private float gravityCurveTransitionTime = 0;


    private float baseGravity;
    private float currentGravity;
    private float gravityMultiplier = 1;

    [Header("Input")]
    public float inputX;
    public bool facingRight = true;
    bool fallingDown;

    public float coyoteDuration;
    [SerializeField] private float coyoteTimer;

    [Header("Components")]
    private Walking walkingScript;
    private PlayerJump jumpingScript;
    private GrappleInput grappleScript;
    private Rigidbody2D rbody;
    private PlayerSpineController spineController;
    private UmbrellaStateHandler umbrellaHandler;

    public enum MovementStates
    {
        GroundMoving,
        Idle,
        AirMoving,
        Gliding, //Not implemented yet
        Knockback,
        Jumping,
        Grappling,
    }

    [SerializeField] public MovementStates currentMoveState;

    void Start()
    {
        walkingScript = GetComponent<Walking>();
        jumpingScript = GetComponent<PlayerJump>();
        grappleScript = GetComponent<GrappleInput>();
        rbody = GetComponent<Rigidbody2D>();
        spineController = GetComponentInChildren<PlayerSpineController>();
        umbrellaHandler = GetComponentInChildren<UmbrellaStateHandler>();

        coyoteTimer = coyoteDuration;
        currentMoveState = MovementStates.GroundMoving;
        currentGravity = baseGravity = rbody.gravityScale;
    }

    bool justGrappled = false;

    void Update()
    {
        if (currentMoveState != MovementStates.Grappling)
        {
            if (justGrappled)
            {
                justGrappled = false;
                CancelInvoke(nameof(DisableGrappleGravity));
                Invoke(nameof(DisableGrappleGravity), 0.5f);
            }

            ManageGravity();
            ManageMovingStates();

            if (inputX > 0)
                facingRight = true;
            else if (inputX < 0)
                facingRight = false;


        }
        else if (currentMoveState == MovementStates.Grappling && grappleScript.canGrapple)
        {
            coyoteTimer = 0;
            currentMoveState = MovementStates.AirMoving;
            grappleGravity = true;
            justGrappled = true;
        }

        falling = rbody.velocity.y > 0.1f ? false : true;
    }

    void ManageGravity()
    {
        if (isGrounded)
        {
            if (!fallingDown)
            {
                currentGravity = baseGravity;
                gravityMultiplier = 1;
            }
            else
                gravityMultiplier = 3;
        }
        else if (pressingJump)
        {
            currentGravity = gravityUpwards;
            gravityMultiplier = 1;
        }
        else // in air and not in jump
        {
            if (rbody.velocity.y > 0 && !grappleGravity)
            {
                gravityMultiplier = 3;
            }
            else if (grappleGravity)
            {
                gravityMultiplier = 0.5f;
            }
            else
            {
                gravityMultiplier = 1;
            }

            gravityCurveTransitionTime += Time.deltaTime;
            currentGravity = Mathf.Lerp(gravityUpwards, downwardGravity, jumpGravityTransitionSpeed.Evaluate(gravityCurveTransitionTime));

        }

        rbody.gravityScale = currentGravity * gravityMultiplier;
    }

    public void DisableGrappleGravity() { grappleGravity = false; }

    void ManageMovingStates()
    {
        if (!midJump)
        {
            if (isGrounded)
            {
                if (!pressingJump)
                    coyoteTimer = coyoteDuration;
                if (inputX != 0)
                {
                    currentMoveState = MovementStates.GroundMoving;
                }
                else
                    currentMoveState = MovementStates.Idle;
            }
            else
            {
                coyoteTimer -= Time.deltaTime;
                currentMoveState = MovementStates.AirMoving;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentMoveState == MovementStates.Jumping)
        {
            jumpingScript.Jump(inputX);

            UpdateAcceleration();

            midJump = false;

        }
        else if (currentMoveState == MovementStates.GroundMoving || currentMoveState == MovementStates.Idle)
        {
            walkingScript.Movement(inputX, isGrounded, onSlope, walkableSlope, slopeDirection);
        }
        else if (currentMoveState == MovementStates.AirMoving)
        {


            walkingScript.Movement(inputX, isGrounded, false, false, Vector2.zero);//Due to air moving not tuching slopes setting its variables to false

            if (falling && slowfalling && umbrellaHandler.currentState == UmbrellaStateHandler.UmbrellaState.Idle)
            {
                umbrellaHandler.slowFalling = true;
                jumpingScript.SlowFalling();
            }
            else
                umbrellaHandler.slowFalling = false;
        }
        else
        {
            walkingScript.UpdateCurrentVelocity();
        }
    }

    public void UpdateAcceleration()
    {
        walkingScript.UpdateCurrentVelocity();
    }

    public void JumpPressed()
    {
        if ((isGrounded || coyoteTimer > 0) && currentMoveState != MovementStates.Grappling)
        {
            pressingJump = true;
            midJump = true;
            coyoteTimer = 0;

            DisableGrappleGravity();
            gravityCurveTransitionTime = 0;
            currentMoveState = MovementStates.Jumping;
            Invoke(nameof(EndJump), maxJumpDuration);
        }
    }

    public void JumpReleased()
    {
        if (pressingJump)
        {
            CancelInvoke();
            pressingJump = false;
        }
    }

    public void FallThroughPlatforms()
    {
        Physics2D.IgnoreLayerCollision(9, 7, true);
        fallingDown = true;
    }

    public void StopFallThroughPlatforms()
    {
        Physics2D.IgnoreLayerCollision(9, 7, false);
        fallingDown = false;
    }


    void EndJump()
    {
        pressingJump = false;
    }

    public void Grapple()
    {
        currentMoveState = MovementStates.Grappling;
    }
}
