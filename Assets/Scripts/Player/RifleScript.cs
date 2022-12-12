using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RifleScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] LayerMask rayIgnore;
    [SerializeField] public LineRenderer aimLaser;
    [HideInInspector] public LineRenderer bulletTrail;
    private SlowMotionHandler slowMo;
    private RaycastHit2D shot;


    [Header("Values")]
    [SerializeField] float trailLength;
    [SerializeField] float aimLaserLength;
    [SerializeField] float shotMaxDistance;
    [SerializeField] float rifleDamage;

    [Header("Vectors")]
    private Vector2 origin;
    private Vector2 mousePos;
    private Vector2 shotDirection;

    private void Start()
    {
        bulletTrail = GetComponent<LineRenderer>(); 
        slowMo = GetComponentInParent<SlowMotionHandler>();
        bulletTrail.enabled = false;
    }

    public IEnumerator Aim()
    {
        aimLaser.enabled = true;
        slowMo.StartCoroutine(slowMo.SlowTime(0.1f));

        while (aimLaser.enabled)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 aimStart = aimLaser.gameObject.transform.position;

            aimLaser.SetPosition(0, aimStart);
            aimLaser.SetPosition(1, aimStart + (mousePos - aimStart).normalized * aimLaserLength);
            yield return null;
        }
    }

    public void ShootRifle()
    {
        aimLaser.enabled = false;
        slowMo.NormalSpeed();

        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        origin = transform.position;

        shotDirection = mousePos - origin;
        shotDirection.Normalize();

        shot = Physics2D.Raycast(origin, shotDirection, shotMaxDistance, rayIgnore);
        if (shot.collider != null && shot.collider.CompareTag("Enemy"))
        {
            try
            {
                shot.collider.GetComponent<HealthHandler>().ReduceHealth(rifleDamage);
            }
            catch (System.Exception ex) { Debug.LogException(ex); }

        }

        bulletTrail.SetPosition(0, origin);
        bulletTrail.SetPosition(1, origin + shotDirection * trailLength);
        bulletTrail.enabled = true;

        StartCoroutine(LineFade());
    }

    private IEnumerator LineFade()
    {
        if(shot.collider != null)
        {

        }
        else
        {

        }
        var gradientHolder = bulletTrail.colorGradient;
        var gradKeys = gradientHolder.alphaKeys;
        while (gradKeys[0].alpha > 0.05f)
        {
            gradKeys[0].alpha -= 0.01f;
            yield return null;
            gradientHolder.alphaKeys = gradKeys;
            bulletTrail.colorGradient = gradientHolder;
        }
        gradKeys[0].alpha = 1;
        gradientHolder.alphaKeys = gradKeys;
        bulletTrail.colorGradient = gradientHolder;
        bulletTrail.enabled = false;
    }
}
