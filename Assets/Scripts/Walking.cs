using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    float inputX;

    //Components
    Rigidbody2D rbody;

    //Inputs
    PlayerInputs playerControls;
    private InputAction move;

    private void Awake()
    {
        playerControls = new PlayerInputs();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        HorizontalMovement();
    }
    public void HorizontalMovement()
    {
        inputX = move.ReadValue<Vector2>().x;
    }

    private void FixedUpdate()
    {
        rbody.velocity = new(inputX * speed * Time.deltaTime, rbody.velocity.y); //Walking()
    }

}
