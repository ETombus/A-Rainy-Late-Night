using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UmbrellaStateHandler;

public class RifleScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] LayerMask rayIgnore;
    [SerializeField] public LineRenderer aimLaser;
    [SerializeField] UmbrellaStateHandler umbrellaHandler;
    [HideInInspector] public LineRenderer bulletTrail;
    private SlowMotionHandler slowMo;
    private RaycastHit2D shot;


    [Header("Values")]
    [SerializeField] float aimLaserLength;
    [SerializeField] float shotMaxDistance;
    [SerializeField] float rifleDamage;
    [SerializeField] float reloadTime;
    [SerializeField] float maxTimeSlowdown;
    [SerializeField] int maxAmmo;
    [SerializeField] int ammoCount;

    [Header("Vectors")]
    private Vector2 origin;
    private Vector2 mousePos;
    private Vector2 shotDirection;

    private void Start()
    {
        bulletTrail = GetComponent<LineRenderer>();
        slowMo = GetComponentInParent<SlowMotionHandler>();
        bulletTrail.enabled = false;
        maxTimeSlowdown /= 2;
    }

    public IEnumerator Aim()
    {
        CancelInvoke();
        aimLaser.enabled = true;
        umbrellaHandler.CancelInvoke();
        umbrellaHandler.StartCoroutine(umbrellaHandler.Timer(maxTimeSlowdown, TimerFillAmount.filled));
        slowMo.StartCoroutine(slowMo.SlowTime(0.1f, maxTimeSlowdown));

        Invoke(nameof(AutoShoot), maxTimeSlowdown);

        while (aimLaser.enabled)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            Vector2 aimStart = aimLaser.gameObject.transform.position;
            var dir = (mousePos - aimStart);
            dir.Normalize();

            var aimRay = Physics2D.Raycast(aimStart, dir, shotMaxDistance, rayIgnore);

            aimLaser.SetPosition(0, aimStart);

            if (aimRay.collider != null)
            {
                aimLaser.SetPosition(1, aimRay.point);
            }
            else
            {
                aimLaser.SetPosition(1, aimStart + (mousePos - aimStart).normalized * aimLaserLength);
            }
            yield return null;
        }
    }

    private void AutoShoot()
    {
        if (aimLaser.enabled)
            ShootRifle();
    }

    public void ShootRifle()
    {
        if (ammoCount > 0)
        {
            umbrellaHandler.StopAllCoroutines();
            umbrellaHandler.sparks.Stop();
            StartCoroutine(umbrellaHandler.Timer(reloadTime, TimerFillAmount.empty));
            aimLaser.enabled = false;
            slowMo.NormalSpeed();
            ammoCount--;

            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            origin = transform.position;

            shotDirection = mousePos - origin;
            shotDirection.Normalize();

            shot = Physics2D.Raycast(origin, shotDirection, shotMaxDistance, rayIgnore);

            if (shot.collider != null)
            {
                if (shot.collider.CompareTag("Enemy"))
                {
                    try
                    {
                        shot.collider.GetComponent<HealthHandler>().ReduceHealth(rifleDamage);
                    }
                    catch (System.Exception ex) { Debug.LogException(ex); }
                }

                shot.transform.SendMessage(nameof(InteractScript.Hit), SendMessageOptions.DontRequireReceiver);
            }

            StartCoroutine(LineFade());
        }
        //else
        //DO SOMETHING
        //GUN JAM?
        //SMOKE?
    }

    private IEnumerator LineFade()
    {
        bulletTrail.SetPosition(0, origin);
        bulletTrail.enabled = true;

        if (shot.collider != null)
        {
            bulletTrail.SetPosition(1, shot.point);
        }
        else
        {
            bulletTrail.SetPosition(1, origin + shotDirection * shotMaxDistance);
        }
        var gradientHolder = bulletTrail.colorGradient;
        var gradKeys = gradientHolder.alphaKeys;
        while (gradKeys[1].alpha > 0.05f)
        {
            gradKeys[1].alpha -= 0.015f;
            yield return null;
            gradientHolder.alphaKeys = gradKeys;
            bulletTrail.colorGradient = gradientHolder;
        }
        gradKeys[1].alpha = 1;
        gradientHolder.alphaKeys = gradKeys;
        bulletTrail.colorGradient = gradientHolder;
        bulletTrail.enabled = false;
    }
}
