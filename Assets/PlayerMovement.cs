using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpVel;
    public bool isGrounded;
    [SerializeField] bool isSlowfalling;
    float inputX;


    //Components
    Rigidbody2D rbody;

    //Inputs
    PlayerInputs playerControls;
    private InputAction move;
    private InputAction jump;

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

    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rbody.velocity.y < 0) { rbody.gravityScale = 5; }
        else if (isGrounded) { rbody.gravityScale = 1; }

        //else { rbody.gravityScale = 1; }

        Walking();
    }

    private void FixedUpdate()
    {
        rbody.velocity = new(inputX * speed * Time.deltaTime, rbody.velocity.y); //Walking()
    }

    public void Walking()
    {
        inputX = move.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            Debug.Log("We Jumped");
            rbody.velocity = new Vector2(rbody.velocity.x, 0);

            FixedUpdate(jumpForce, jumpVel);
        }
    }

    private void FixedUpdate(float jumpValue, float jumpVelocity)
    {
        rbody.velocity = new(rbody.velocity.x, jumpVelocity * Time.deltaTime);
        rbody.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse); //Jump()
    }
}
