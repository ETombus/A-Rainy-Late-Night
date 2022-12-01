using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using static System.TimeZoneInfo;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("Jump variables")]
    [SerializeField] private bool isGrounded;
    private bool midJump = false;
    private float transitionTime = 0;
    public float maxJumpDuration = 0.5f;
    public AnimationCurve jumpgravityTransitionSpeed;
    private bool pressingJump = false;
    private bool grappleJump = false;

    [Header("Gravity")]
    public float gravityUpwards = 1;
    public float downwardGravity = 1;
    private float baseGravity;
    private float currentGravity;
    private float gravityMultiplier = 1;

    [Header("Input")]
    public float inputX;
    PlayerInputs playerControls;
    InputAction grappleAction;
    private InputAction move;
    private InputAction jump;

    [Header("Components")]
    private Walking walkingScript;
    private PlayerJump jumpingScript;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rbody;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float extraLeangth = 1;

    public enum MovementStates
    {
        GroundMoving,
        Idle,
        AirMoving,
        Gliding,
        Jumping,
        MidJumping,
        Grappling
    }

    [SerializeField] public MovementStates currentMoveState;

    private void Awake()
    {
        playerControls = new PlayerInputs();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        jump.performed += SlowFalling;
        jump.canceled += OnSpaceReleased;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }


    void Start()
    {
        walkingScript = GetComponent<Walking>();
        jumpingScript = GetComponent<PlayerJump>();
        boxCollider = GetComponent<BoxCollider2D>();
        rbody = GetComponent<Rigidbody2D>();

        currentMoveState = MovementStates.GroundMoving;
        currentGravity = baseGravity = rbody.gravityScale;
    }

    void Update()
    {
        ManageInputs();
        isGrounded = IsGrounded();

        if (Grapple.stuck && currentMoveState != MovementStates.Jumping)
        {
            currentMoveState = MovementStates.Grappling;
        }

        if (currentMoveState != MovementStates.Grappling)
        {
            ManageGravity();
            ManageMovingStates();
        }
        else if (currentMoveState == MovementStates.Grappling && !Grapple.stuck)
        {
            currentMoveState = MovementStates.AirMoving;
        }
    }

    void ManageGravity()
    {
        if (isGrounded)
        {
            Debug.Log("Ground gravity");
            currentGravity = baseGravity;
        }
        else if (pressingJump)
        {
            Debug.Log("Jump gravity");
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

            transitionTime += Time.deltaTime;
            currentGravity = Mathf.Lerp(gravityUpwards, downwardGravity, jumpgravityTransitionSpeed.Evaluate(transitionTime));

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

    public void ManageInputs()
    {
        inputX = move.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            pressingJump = true;
            midJump = true;
            transitionTime = 0;
            currentMoveState = MovementStates.Jumping;
            Invoke(nameof(EndJump), maxJumpDuration);
        }

        if (currentMoveState == MovementStates.Grappling)
        {
            grappleJump = true;
            pressingJump = true;
            midJump = true;
            transitionTime = 0;
            currentMoveState = MovementStates.Jumping;
            Invoke(nameof(EndJump), maxJumpDuration);
        }
        else
            grappleJump = false;
    }

    void EndJump()
    {
        pressingJump = false;
    }

    private void SlowFalling(InputAction.CallbackContext context)
    {

    }
    private void OnSpaceReleased(InputAction.CallbackContext context)
    {
        if (pressingJump)
        {

            CancelInvoke();
            pressingJump = false;
        }
    }

    private void FixedUpdate()
    {

        if (currentMoveState == MovementStates.Jumping)
        {
            jumpingScript.Jump(grappleJump ? Grapple.directionX * 2.5f : inputX);

            walkingScript.UpdateCurrentVelocity();
            midJump = false;
        }
        else if (currentMoveState == MovementStates.GroundMoving || 
            currentMoveState == MovementStates.Idle || currentMoveState == MovementStates.AirMoving)
        {
            walkingScript.Movement(inputX, isGrounded);
        }

    }

    private bool IsGrounded()
    {
        RaycastHit2D rayHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraLeangth, groundLayer);
        Color rayColor;
        if (rayHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }


        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraLeangth), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraLeangth), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + extraLeangth), Vector2.right * (boxCollider.bounds.extents.x * 2), rayColor);


        return rayHit.collider != null;
    }


}
