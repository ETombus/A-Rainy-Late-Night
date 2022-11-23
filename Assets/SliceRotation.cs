using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SliceRotation : MonoBehaviour
{
    Vector3 mousePos;
    float angle;

    Slice slice;

    private void Start()
    {
        slice = GetComponentInParent<Slice>();
    }

    private void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.transform.position.z;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (slice.canAttack)
        {
            transform.rotation = Quaternion.Euler(new(0, 0, angle));
        }
    }
}
