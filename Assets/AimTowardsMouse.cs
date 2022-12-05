using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AimTowardsMouse : MonoBehaviour
{
    Vector2 mousePos;
    float angle;



    FlipPlayer flipX;
    Slice slice;

    private void Start()
    {
        slice = GetComponentInParent<Slice>();
        flipX = FindObjectOfType<FlipPlayer>();
    }

    private void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        Vector2 camMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        flipX.MoveCameraInbetween(camMousePos);

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        if (mousePos.x < transform.position.x - 100f && !FlipPlayer.flippedX)
        {
            flipX.FlipPlayerX();
            FlipPlayer.flippedX = true;
        }
        else if (mousePos.x > transform.position.x + 100f && FlipPlayer.flippedX)
        {
            flipX.FlipPlayerX();
            FlipPlayer.flippedX = false;
            FlipPlayer.overrideFlip = true;
        }

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (slice.canAttack)
        {
            transform.rotation = Quaternion.Euler(new(0, 0, angle));
        }
    }
}
