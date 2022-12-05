using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input")]
    PlayerInputs playerControls;
    InputAction grappleAction;
    private InputAction move;
    private InputAction jump;

    private PlayerStateHandler stateHandler;


    private void Awake()
    {
        playerControls = new PlayerInputs();

    }
    private void Start()
    {
        stateHandler = GetComponent<PlayerStateHandler>();
    }

    private void Update()
    {
        HorizontalInputs();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        //jump.performed += SlowFalling;
        jump.canceled += OnSpaceReleased;

    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    public void HorizontalInputs()
    {
        stateHandler.inputX = move.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        stateHandler.JumpPressed();
    }


    //private void SlowFalling(InputAction.CallbackContext context)
    //{

    //}

    private void OnSpaceReleased(InputAction.CallbackContext context)
    {
        stateHandler.JumpReleased();
    }


}
