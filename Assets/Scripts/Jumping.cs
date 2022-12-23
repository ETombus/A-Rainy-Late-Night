using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpVel;
    [SerializeField] bool isJumping;

    [Header("Slowfall")]
    public bool isGrounded;
    [SerializeField] bool isSlowfalling;
    [SerializeField] float gravityOffset = 1f;
    [SerializeField] float slowfallGravity = 0.2f;
    [SerializeField] float fastfallGravity = 5f;

    //[Header("Duration")] //this might not do anything right now, idk how the new input system works :/
    //[SerializeField] float jumpDuration;
    //float jumpTimer = 0f;

    //[Header("Coyote Time")]
    //[SerializeField] float coyoteTime = 0.2f;
    //[SerializeField] float coyoteTimer;

    //[Header("Jump buffer")]
    //[SerializeField] float jumpbufferTime = 0.2f;
    //[SerializeField] float jumpbufferTimer;

    //Components
    Rigidbody2D rbody;
    UmbrellaOpener umbrella;
    //Inputs
    PlayerInputs playerControls;
    private InputAction jump;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        playerControls = new PlayerInputs();
        umbrella = GetComponentInChildren<UmbrellaOpener>();
    }

    private void OnEnable()
    {
        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        jump.performed += SlowFalling;
        jump.canceled += OnSpaceReleased;
    }

    private void OnDisable()
    {
        jump.Disable();
    }

    private void Update()
    {
        if (rbody.velocity.y < 0) //checking if the player is falling, then applying slowfall if necessary
        {
            if (isSlowfalling)
            {
                gravityOffset = slowfallGravity;
                umbrella.umbrellaOverrideBool = true;
                umbrella.OpenUmbrella();
            }
            else if (!isSlowfalling && !GrappleOLD.stuck) { gravityOffset = fastfallGravity; }
            else if (GrappleOLD.stuck) {gravityOffset = slowfallGravity;}
        }

        if (isGrounded)
        {
            gravityOffset = 1f;
            isSlowfalling = false;
        }
        rbody.gravityScale = gravityOffset;


        if (!isGrounded)
        {
            isJumping = false;

            //if (coyoteTimer > 0) coyoteTimer -= Time.deltaTime;
        }

        //if (jumpbufferTimer > 0) {jumpbufferTimer -= Time.deltaTime;}
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            //Debug.Log("We Jumped");
            rbody.velocity = new Vector2(rbody.velocity.x, 0);
            isJumping = true;


        }
    }

    public void ResetJump(InputAction.CallbackContext context) { isJumping = false; }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            rbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void SlowFalling(InputAction.CallbackContext context)
    {
        //Debug.Log("slowfalling");
        isSlowfalling = true;
    }

    private void OnSpaceReleased(InputAction.CallbackContext context)
    {
        umbrella.umbrellaOverrideBool = false;
        isSlowfalling = false;
        rbody.velocity = new(rbody.velocity.x, rbody.velocity.y * 0.5f);
    }
}
