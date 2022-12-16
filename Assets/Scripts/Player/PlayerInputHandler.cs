using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input")]
    PlayerInputs playerControls;
    InputAction grappleAction; // TOM WTHELL
    private InputAction move;
    private InputAction jump;
    private InputAction fall;
    [SerializeField] private bool fallDown = false;

    [Header("UmbrellaInputs")]
    private InputAction aim;
    private InputAction shoot;
    private InputAction slash;
    private InputAction grapple;

    [Header("Scripts")]
    private RifleScript rifleScript;
    private PlayerStateHandler stateHandler;
    private UmbrellaStateHandler umbrellaHandler;

    private void Awake() { playerControls = new PlayerInputs(); }

    private void Start()
    {
        stateHandler = GetComponent<PlayerStateHandler>();
        umbrellaHandler = GetComponentInChildren<UmbrellaStateHandler>();
        rifleScript = GetComponentInChildren<RifleScript>();
    }

    private void Update()
    {
        HorizontalInputs();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Horizontal;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        jump.performed += SlowFalling;
        jump.canceled += OnSpaceReleased;

        fall = playerControls.Player.Fall;
        fall.Enable();
        fall.performed += ActivateFall;
        fall.canceled += DisableFall;

        aim = playerControls.Player.Aim;
        aim.Enable();
        aim.performed += AimHandler;
        aim.canceled += AimHandler;

        shoot = playerControls.Player.SlashShoot;
        shoot.Enable();
        shoot.performed += Shoot;

        slash = playerControls.Player.SlashShoot;
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
        fall.Disable();

        aim.Disable();
        shoot.Disable();
        slash.Disable();
        grapple.Disable();
    }

    public void HorizontalInputs()
    {
        stateHandler.inputX = move.ReadValue<float>();
    }

    public void ActivateFall(InputAction.CallbackContext context) { fallDown = true; }
    public void DisableFall(InputAction.CallbackContext context) 
    { 
        fallDown = false; 
        stateHandler.StopFallThroughPlatforms();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!fallDown)
            stateHandler.JumpPressed();
        else
        {
            stateHandler.FallThroughPlatforms();
        }

    }


    private void SlowFalling(InputAction.CallbackContext context)
    {
        stateHandler.slowfalling = true;

    }

    private void OnSpaceReleased(InputAction.CallbackContext context)
    {
        stateHandler.JumpReleased();
        stateHandler.slowfalling = false;
    }

    public void AimHandler(InputAction.CallbackContext context)
    {
        if (context.performed && !umbrellaHandler.reloading && umbrellaHandler.currentState == UmbrellaStateHandler.UmbrellaState.Idle)
        {
            StartCoroutine(rifleScript.Aim());
            umbrellaHandler.currentState = UmbrellaStateHandler.UmbrellaState.Aiming;
        }
        else if (context.canceled && umbrellaHandler.currentState == UmbrellaStateHandler.UmbrellaState.Aiming)
        {
            rifleScript.aimLaser.enabled = false;
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
