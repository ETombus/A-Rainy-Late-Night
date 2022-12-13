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
    [SerializeField] private Slider clockSlider;
    [SerializeField] private LayerMask rayIgnore;
    private SlowMotionHandler slowMo;
    private GrappleInput grapplingHook;
    private Collider2D umbrellaCollider;

    [Header("Values")]
    [SerializeField] private float maxRainHeightCheck;
    [SerializeField] private float rainDamageInterval;
    [SerializeField] private float rainDamage;
    [SerializeField] private float slashDelay;

    [Header("Audio")]
    public PlayerSoundHandler soundHandler;
    public AudioClip[] clips;
    //0 - slash, 1 - grapple, 2 - shoot

    [Header("Bools")]
    public bool reloading = false;
    public bool inRain = false;
    public bool umbrellaUp = false;
    public bool slowFalling = false;

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
        slowMo = player.GetComponent<SlowMotionHandler>();

        soundHandler = GetComponentInParent<PlayerSoundHandler>();

        currentState = UmbrellaState.Idle;
        clockSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        Debug.Log(currentState);
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
        else if (currentState == UmbrellaState.Idle)
        {
            //unequip umbrella

            umbrellaUp = slowFalling ? true : false;
        }
    }

    private IEnumerator RainDamage()
    {
        while (inRain)
        {
            player.GetComponent<Healthbar>().ReduceHealth(rainDamage);
            yield return new WaitForSeconds(rainDamageInterval);
        }
    }

    public void Shoot()
    {
        if (currentState == UmbrellaState.Aiming)
        {
            rifle.GetComponent<RifleScript>().ShootRifle();

            soundHandler.PlaySound(clips[2]);
        }
    }

    public IEnumerator Reload(float time, bool fill)
    {
        clockSlider.gameObject.SetActive(true);
        float fillOverTime = clockSlider.maxValue / time;
        reloading = true;

        if (fill)
        {
            yield return null;
            clockSlider.value = clockSlider.minValue;
            Idle();
        }
        else
        {
            clockSlider.value = clockSlider.maxValue;
            fillOverTime *= -1;
        }

        while (fill ? clockSlider.value < clockSlider.maxValue : clockSlider.value > clockSlider.minValue)
        {
            clockSlider.value += fillOverTime * Time.deltaTime;
            yield return null;
        }

        reloading = false;
        clockSlider.value = 0f;
        clockSlider.gameObject.SetActive(false);
    }

    public void Slash()
    {
        if (currentState == UmbrellaState.Idle && !grapplingHook.targetLocked || currentState == UmbrellaState.Grapple)
        {
            currentState = UmbrellaState.Slash;
            GetComponentInParent<Slice>().StandardSlice();
            Invoke(nameof(Idle), slashDelay);

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
        slowMo.NormalSpeed();
        currentState = UmbrellaState.Idle;
    }
}