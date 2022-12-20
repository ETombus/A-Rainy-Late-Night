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
    [SerializeField] private LayerMask rayIgnore;
    [SerializeField] public Slider clockSlider;

    private EdgeCollider2D umbrellaCollider;

    [Header("Scripts")]
    private PlayerStateHandler stateHandler;
    private GrappleInput grapplingHook;
    private SlowMotionHandler slowMo;
    private RifleScript rifleCS;

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
    public bool timerOn = false;
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

    public enum TimerFillAmount
    {
        filled,
        empty,
        current
    }

    private void Start()
    {
        stateHandler = GetComponentInParent<PlayerStateHandler>();
        grapplingHook = GetComponentInParent<GrappleInput>();
        slowMo = player.GetComponent<SlowMotionHandler>();
        rifleCS = rifle.GetComponent<RifleScript>();

        soundHandler = GetComponentInParent<PlayerSoundHandler>();
        umbrellaCollider = GetComponent<EdgeCollider2D>();

        currentState = UmbrellaState.Idle;
        clockSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        var rayHit = Physics2D.Raycast(player.transform.position, Vector2.up, maxRainHeightCheck, rayIgnore).collider;

        if (rayHit == null || rayHit.CompareTag(rainColliderTag.tag))
        {
            if (currentState == UmbrellaState.Idle)
            {
                umbrellaCollider.enabled = true;

                umbrellaUp = true;
                inRain = false;
            }
            else if (!inRain)
            {
                umbrellaCollider.enabled = false;

                inRain = true;
                StartCoroutine(RainDamage());
            }
        }
        else if (currentState == UmbrellaState.Idle)
        {
            umbrellaCollider.enabled = false;
            umbrellaUp = slowFalling ? true : false;
        }

        if (stateHandler.inputX > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (stateHandler.inputX < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
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
            rifleCS.ShootRifle();

            soundHandler.PlaySound(clips[2]);
        }
    }

    public IEnumerator Timer(float time, TimerFillAmount type)
    {
        clockSlider.gameObject.SetActive(true);
        float fillOverTime = clockSlider.maxValue / time;
        timerOn = true;

        if (type == TimerFillAmount.empty)
        {
            yield return null;
            clockSlider.value = clockSlider.minValue;
            Idle();

            while (clockSlider.value < clockSlider.maxValue)
            {
                clockSlider.value += fillOverTime * Time.deltaTime;
                yield return null;
            }
        }
        else if (type == TimerFillAmount.filled)
        {
            clockSlider.value = clockSlider.maxValue;
            fillOverTime *= -1;

            while (clockSlider.value > clockSlider.minValue)
            {
                clockSlider.value += fillOverTime * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (clockSlider.value < clockSlider.maxValue)
            {
                clockSlider.value += fillOverTime * Time.deltaTime;
                yield return null;
            }
        }

        timerOn = false;
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