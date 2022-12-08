using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaStateHandler : MonoBehaviour
{
    private Collider2D umbrellaCollider;
    private Animator umbrellaVisualState;

    [SerializeField] private float maxRainHeightCheck;
    [SerializeField] private LayerMask rayIgnore;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject rainColliderTag;

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
        var rayHit = Physics2D.Raycast(player.transform.position, Vector2.up, maxRainHeightCheck, rayIgnore).collider;

        if (rayHit == null || rayHit.CompareTag(rainColliderTag.tag))
        {
            if(currentState == UmbrellaState.Idle)
            {
                //equip umbrella
            }
            else
            {
                //Take rain damage
            }
        }
        else
        {
            //unequip umbrella
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
            Invoke(nameof(Idle), 0.35f);
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