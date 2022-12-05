using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using static System.TimeZoneInfo;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("Jump variables")]
    public bool isGrounded;

    private bool pressingJump = false;
    private bool midJump = false;

    public float maxJumpDuration = 0.5f;

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
    
    [Header("Components")]
    private Walking walkingScript;
    private PlayerJump jumpingScript;
    private Rigidbody2D rbody;


    public enum MovementStates
    {
        GroundMoving,
        Idle,
        AirMoving,
        Gliding, //Not implemented yet
        Jumping,
        Grappling
    }

    [SerializeField] public MovementStates currentMoveState;

    void Start()
    {
        walkingScript = GetComponent<Walking>();
        jumpingScript = GetComponent<PlayerJump>();
        rbody = GetComponent<Rigidbody2D>();

        currentMoveState = MovementStates.GroundMoving;
        currentGravity = baseGravity = rbody.gravityScale;
    }

    void Update()
    {
        if (currentMoveState != MovementStates.Grappling)
        {
            ManageGravity();
            ManageMovingStates();
        }
        else if (currentMoveState == MovementStates.Grappling)
        {
            currentMoveState = MovementStates.AirMoving;
        }
    }

    void ManageGravity()
    {
        if (isGrounded)
        {
            currentGravity = baseGravity;
        }
        else if (pressingJump)
        {

            currentGravity = gravityUpwards;
            gravityMultiplier = 1;
        }
        else // in air and not in jump
        {
            if (rbody.velocity.y > 0)
            {
                gravityMultiplier = 2;
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

    void ManageMovingStates()
    {
        if (!midJump)
        {
            if (isGrounded)
            {
                if (inputX != 0)
                {
                    currentMoveState = MovementStates.GroundMoving;
                }
                else
                    currentMoveState = MovementStates.Idle;
            }
            else
            {
                currentMoveState = MovementStates.AirMoving;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentMoveState == MovementStates.Jumping)
        {
            jumpingScript.Jump(inputX);

            walkingScript.UpdateCurrentVelocity();
            midJump = false;
        }
        else if (currentMoveState == MovementStates.GroundMoving || 
            currentMoveState == MovementStates.Idle || currentMoveState == MovementStates.AirMoving)
        {
            walkingScript.Movement(inputX, isGrounded);
        }
    }


    public void JumpPressed()
    {
        if (isGrounded)
        {
            pressingJump = true;
            midJump = true;
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

    void EndJump()
    {
        pressingJump = false;
    }
}
