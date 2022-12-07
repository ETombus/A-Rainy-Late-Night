using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaStateHandler : MonoBehaviour
{
    private Collider2D umbrellaCollider;
    private Animator umbrellaVisualState;

    [SerializeField] private float maxRainHeightCheck;
    [SerializeField] private LayerMask ignorePlayer;
    [SerializeField] private GameObject player;

    private bool umbrellaOpen = true;

    public UmbrellaState currentState;

    private GrappleInput grapplingHook;

    public enum UmbrellaState
    {
        Idle,
        Aiming,
        Shoot,
        Grapple,
        Slash,
    }

    private void Start()
    {
        umbrellaCollider = GetComponent<Collider2D>();
        umbrellaVisualState = GetComponent<Animator>();
        grapplingHook = GetComponentInParent<GrappleInput>();

        currentState = UmbrellaState.Idle;
    }

    private void Update()
    {
        if (!umbrellaOpen
            && Physics2D.Raycast(player.transform.position, Vector2.up, maxRainHeightCheck, ignorePlayer).collider == null)
        {
            //Take rain damage
        }
    }

    public void Shoot()
    {
        if (currentState == UmbrellaState.Aiming)
        {
            //Shoot
        }
    }

    public void Slash()
    {
        if (currentState == UmbrellaState.Idle)
        {
            currentState = UmbrellaState.Slash;
            GetComponentInParent<Slice>().StandardSlice();
            Invoke(nameof(Idle), 0.25f);
        }
    }

    public void Grapple()
    {
        if (currentState == UmbrellaState.Idle)
        {
            currentState = UmbrellaState.Grapple;
            grapplingHook.ShootGrapple();
        }
    }

    public void Idle()
    {
        currentState = UmbrellaState.Idle;
    }
}