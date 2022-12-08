using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RifleScript : MonoBehaviour
{
    private RaycastHit2D shot;
    private Vector2 origin;
    private Vector2 mousePos;
    private Vector2 shotDirection;
    private LineRenderer bulletTrail;

    private void Start()
    {
        bulletTrail= GetComponent<LineRenderer>();
        bulletTrail.enabled = false;
        ShootRifle();
    }

    public void ShootRifle()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        origin = transform.position;

        shotDirection = mousePos - origin;
        shotDirection.Normalize();

        shot = Physics2D.Raycast(origin, mousePos);

        bulletTrail.SetPosition(0, origin);
        bulletTrail.SetPosition(1, shot.point);
        bulletTrail.enabled = true;

    }
}
