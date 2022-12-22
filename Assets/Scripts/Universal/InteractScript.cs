using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider2D))]

public class InteractScript : MonoBehaviour
{
    [SerializeField] private UnityEvent function;

    [Header("Components")]
    [Tooltip("If true object will be interactable through input\nif false will be interactable through damage")]
    [SerializeField] private bool isInteractable;
    [Tooltip("Only used with PlaySound() Method")]
    [SerializeField] AudioClip[] clip;
    private CircleCollider2D interactCollider;
    private bool interactTrigger;
    public Collider2D colliderTournOff;

    [Header("Input")]
    private PlayerInputs playerControls;
    private InputAction interact;

    [Header("Values")]
    [Tooltip("Only needed if isInteractable")]
    [SerializeField] private float interactDistance = 2;

    private void Awake() { playerControls = new PlayerInputs(); }

    private void Start()
    {
        if (isInteractable)
        {
            interactCollider = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
            interactCollider.isTrigger = true;
            interactCollider.radius = interactDistance;
        }
        else if (gameObject.layer == 0)
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
        if (isInteractable && collision.transform.parent != null && collision.transform.parent.CompareTag("Player"))
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
    public void DebugFunction() { Debug.Log("Interact"); }
    public void DisableScript() { enabled = false; }
    public void DestroyObject() { Destroy(gameObject); }
    public void ParticleEffect(ParticleSystem particle) { particle.Play(); }

    [Tooltip("Requires Audioclip component")]
    public void PlaySound(AudioSource source)
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(clip[Random.Range(0, clip.Length - 1)]);
    }
    [Tooltip("Requires Audioclip component")]
    public void PlaySound()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(clip[Random.Range(0, clip.Length - 1)]);
    }
    public void ToggleCollider()
    {
        if (colliderTournOff.enabled)
            colliderTournOff.enabled = false;
        else
            colliderTournOff.enabled = true;
    }
}
