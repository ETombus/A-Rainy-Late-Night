using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]

public class InteractScript : MonoBehaviour
{
    [SerializeField] private UnityEvent function;

    [Header("Components")]
    [Tooltip("If true object will be interactable through input\nif false will be interactable through damage")]
    [SerializeField] private bool isInteractable;
    private CircleCollider2D interactCollider;
    private bool interactTrigger;

    [Header("Input")]
    private PlayerInputs playerControls;
    private InputAction interact;

    [Header("Values")]
    [Tooltip("Only needed if isInteractable")]
    [SerializeField] private float interactDistance;

    private void Awake() { playerControls = new PlayerInputs(); }

    private void Start()
    {
        if (isInteractable)
        {
            GetComponent<Collider2D>().isTrigger = true;
            gameObject.layer = 0;
            interactCollider = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
            interactCollider.isTrigger = true;
            interactCollider.radius = interactDistance;
        }
        else
            gameObject.layer = 8;
    }

    private void OnEnable()
    {
        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += InteractPress;
        interact.canceled += InteractPress;
    }

    void InteractPress(InputAction.CallbackContext context)
    {
        if (context.performed)
            interactTrigger = true;
        else if (context.canceled)
            interactTrigger = false;
    }

    //Interact within range
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInteractable && collision.CompareTag("Player"))
        {
            //Visual indication of interactable

            if (interactTrigger)
            {
                interactTrigger = false;
                ActivateFunction();
            }
        }
    }

    public void Hit()
    {
        if (!isInteractable)
            ActivateFunction();
    }

    private void ActivateFunction()
    {
        if (enabled)
            function.Invoke();
    }

    public void DisableScript() { enabled = false; }
}
