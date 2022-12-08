using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UmbrellaStateHandler : MonoBehaviour
{
    private Collider2D umbrellaCollider;
    private Animator umbrellaVisualState;

    [SerializeField] private float maxRainHeightCheck;
    [SerializeField] private Slider reloadSlider;
    [SerializeField] private float reloadTime;
    [SerializeField] private LayerMask rayIgnore;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject rainColliderTag;

    public bool reloading = false;
    

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
        reloadSlider.gameObject.SetActive(false);
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
            Debug.Log("shoot");
            //Shoot
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        currentState = UmbrellaState.Shoot;
        yield return null;
        Idle();

        reloading = true;
        reloadSlider.gameObject.SetActive(true);

        float fillOverTime = reloadSlider.maxValue / reloadTime;
        while(reloadSlider.value<reloadSlider.maxValue)
        {
            reloadSlider.value += fillOverTime * Time.deltaTime;
            yield return null;
        }

        reloading = false;
        reloadSlider.value = 0f;
        reloadSlider.gameObject.SetActive(false);
    }

    public void Slash()
    {
        if (currentState == UmbrellaState.Idle)
        {
            //Debug.Log("sus");
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