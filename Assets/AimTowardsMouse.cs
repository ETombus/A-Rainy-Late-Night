using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AimTowardsMouse : MonoBehaviour
{
    FlipPlayer flipX;
    bool flippedX;
    Vector3 mousePos;
    float angle;

    Slice slice;

    private void Start()
    {
        slice = GetComponentInParent<Slice>();
        flipX = FindObjectOfType<FlipPlayer>();
    }

    private void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.transform.position.z;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        var zRotation = transform.localRotation.eulerAngles.z;

        if (mousePos.x < transform.position.x && !FlipPlayer.flippedX && !FlipPlayer.overrideFlip)
        {
            flipX.FlipPlayerX();
            FlipPlayer.flippedX = true;
        }
        else if (mousePos.x > transform.position.x && FlipPlayer.flippedX && !FlipPlayer.overrideFlip)
        {
            flipX.FlipPlayerX();
            FlipPlayer.flippedX = false;
        }

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (slice.canAttack)
        {
            transform.rotation = Quaternion.Euler(new(0, 0, angle));
        }
    }
}
