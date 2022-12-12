using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UmbrellaStateHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject rainColliderTag;
    [SerializeField] private Slider reloadSlider;
    [SerializeField] private LayerMask rayIgnore;
    private GrappleInput grapplingHook;
    private Collider2D umbrellaCollider;

    [Header("Values")]
    [SerializeField] private float maxRainHeightCheck;
    [SerializeField] private float reloadTime;
    [SerializeField] private int rainDamage;

    [Header("Audio")]
    public PlayerSoundHandler soundHandler;
    public AudioClip[] clips;
    //0 - slash, 1 - grapple, 2 - shoot

    [Header("Bools")]
    public bool reloading = false;
    public bool inRain = false;
    public bool umbrellaUp = false;

    [Header("Enum")]
    public UmbrellaState currentState;

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
        grapplingHook = GetComponentInParent<GrappleInput>();

        soundHandler = GetComponentInParent<PlayerSoundHandler>();

        currentState = UmbrellaState.Idle;
        reloadSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        var rayHit = Physics2D.Raycast(player.transform.position, Vector2.up, maxRainHeightCheck, rayIgnore).collider;

        if (rayHit == null || rayHit.CompareTag(rainColliderTag.tag))
        {
            if (currentState == UmbrellaState.Idle)
            {
                //equip umbrella
                umbrellaUp = true;
                inRain = false;
            }
            else if (!inRain)
            {
                inRain = true;
                StartCoroutine(RainDamage());
            }
        }
        else if(currentState == UmbrellaState.Idle)
        {
            //unequip umbrella
            umbrellaUp = false;
        }
    }

    private IEnumerator RainDamage()
    {
        while (inRain)
        {
            player.GetComponent<Healthbar>().ReduceHealth(rainDamage);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Shoot()
    {
        if (currentState == UmbrellaState.Aiming)
        {
            Debug.Log("shoot");
            rifle.GetComponent<RifleScript>().ShootRifle();
            StartCoroutine(Reload());

            soundHandler.PlaySound(clips[2]);
        }
    }

    private IEnumerator Reload()
    {
        yield return null;
        Idle();

        reloading = true;
        reloadSlider.gameObject.SetActive(true);

        float fillOverTime = reloadSlider.maxValue / reloadTime;
        while (reloadSlider.value < reloadSlider.maxValue)
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
            currentState = UmbrellaState.Slash;
            GetComponentInParent<Slice>().StandardSlice();
            Invoke(nameof(Idle), 0.35f);

            soundHandler.PlaySound(clips[0]);
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
        inRain = false;
        currentState = UmbrellaState.Idle;
    }
}