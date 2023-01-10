using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UmbrellaStateHandler;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInputs playerControls;
    InputAction grappleAction; // TOM WTHELL
    private InputAction move;
    private InputAction jump;
    private InputAction fall;
    [SerializeField] private bool fallDown = false;
    [SerializeField] PlayerInput playerInput;

    [Header("UmbrellaInputs")]
    private InputAction aim;
    private InputAction shoot;
    private InputAction slash;
    private InputAction grapple;

    [Header("Scripts")]
    private RifleScript rifleScript;
    private PlayerStateHandler stateHandler;
    private UmbrellaStateHandler umbrellaHandler;
    public InputActionAsset inputActions;


    private void Awake()
    {
        playerControls = new PlayerInputs();
        playerInput = GetComponent<PlayerInput>();
    }

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
        move = playerInput.actions["Horizontal"];
        move.Enable();

        jump = playerInput.actions["Jump"];
        jump.Enable();
        jump.performed += Jump;
        jump.performed += SlowFalling;
        jump.canceled += OnSpaceReleased;

        fall = playerInput.actions["Fall"];
        fall.Enable();
        fall.performed += ActivateFall;
        fall.canceled += DisableFall;

        aim = playerInput.actions["Aim"];
        aim.Enable();
        aim.performed += AimHandler;
        aim.canceled += AimHandler;

        shoot = playerInput.actions["SlashShoot"];
        shoot.Enable();
        shoot.performed += Shoot;

        slash = playerInput.actions["SlashShoot"];
        slash.Enable();
        slash.performed += Slash;

        grapple = playerInput.actions["Grapple"];
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

    public void ActivateFall(InputAction.CallbackContext context) 
    { 
        stateHandler.FallThroughPlatforms();
    }
    public void DisableFall(InputAction.CallbackContext context)
    {
        stateHandler.StopFallThroughPlatforms();
    }

    public void Jump(InputAction.CallbackContext context)
    {
            stateHandler.JumpPressed();
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
        if (context.performed && !umbrellaHandler.timerOn && umbrellaHandler.currentState == UmbrellaStateHandler.UmbrellaState.Idle)
        {
            StartCoroutine(rifleScript.Aim());
            umbrellaHandler.currentState = UmbrellaStateHandler.UmbrellaState.Aiming;
        }
        else if (context.canceled && umbrellaHandler.currentState == UmbrellaStateHandler.UmbrellaState.Aiming)
        {
            float clockValue = umbrellaHandler.clockSlider.value;
            umbrellaHandler.StopAllCoroutines();
            umbrellaHandler.TurnOffSparks();
            umbrellaHandler.StartCoroutine(umbrellaHandler.Timer(GetComponentInParent<SlowMotionHandler>().cooldown.Evaluate(clockValue), TimerFillAmount.current));
            umbrellaHandler.clockSlider.value = clockValue;
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
