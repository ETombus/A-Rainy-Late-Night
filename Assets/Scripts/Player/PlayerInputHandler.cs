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

    [Header("UmbrellaInputs")]
    private InputAction aim;
    private InputAction shoot;
    private InputAction slash;
    private InputAction grapple;

    private PlayerStateHandler stateHandler;
    private UmbrellaStateHandler umbrellaHandler;

    private void Awake()
    {
        playerControls = new PlayerInputs();

    }
    private void Start()
    {
        stateHandler = GetComponent<PlayerStateHandler>();
        umbrellaHandler = GetComponentInChildren<UmbrellaStateHandler>();
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

        aim = playerControls.Player.Aim;
        aim.Enable();
        aim.performed += AimHandler;
        aim.canceled += AimHandler;

        shoot = playerControls.Player.Fire;
        shoot.Enable();
        shoot.performed += Shoot;

        slash = playerControls.Player.Fire;
        slash.Enable();
        slash.performed += Slash;

        grapple = playerControls.Player.Grapple;
        grapple.Enable();
        grapple.performed += Grapple;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();

        aim.Disable();
        shoot.Disable();
        slash.Disable();
        grapple.Disable();
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

    public void AimHandler(InputAction.CallbackContext context)
    {
        if (context.performed && !umbrellaHandler.reloading)
        {
            Debug.Log("aim");
            umbrellaHandler.currentState = UmbrellaStateHandler.UmbrellaState.Aiming;
        }
        else if (context.canceled && umbrellaHandler.currentState == UmbrellaStateHandler.UmbrellaState.Aiming)
        {
            Debug.Log("down aim");
            umbrellaHandler.Idle();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        umbrellaHandler.Shoot();
    }

    public void Slash(InputAction.CallbackContext context)
    {
        umbrellaHandler.Slash();
    }

    public void Grapple(InputAction.CallbackContext context)
    {
        umbrellaHandler.Grapple();
    }
}
