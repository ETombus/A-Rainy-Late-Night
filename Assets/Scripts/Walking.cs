using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Walking : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    float inputX;

    [Header("Components")]
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Transform cameraTarget;
    [SerializeField] float cameraTargetXPos;
    [SerializeField] Transform umbrellaTrans;
    Vector2 umbrellaPos;
    Rigidbody2D rbody;
    FlipPlayer flipX;
    bool playerFlipped;

    //Inputs
    PlayerInputs playerControls;
    private InputAction move;

    private void Awake()
    {
        playerControls = new PlayerInputs();
        playerSprite = GetComponentInChildren<SpriteRenderer>(false);

        cameraTargetXPos = cameraTarget.localPosition.x;
        umbrellaPos = umbrellaTrans.localPosition;
        flipX = GetComponent<FlipPlayer>();
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

        if (inputX < 0 && !FlipPlayer.flippedX)
        {
            flipX.FlipPlayerX();
            FlipPlayer.flippedX = true;
        }
        else if (inputX > 0 && FlipPlayer.flippedX)
        {
            flipX.FlipPlayerX();
            FlipPlayer.flippedX = false;
        }
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
