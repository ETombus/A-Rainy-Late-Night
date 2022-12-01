using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHookInputs : MonoBehaviour
{
    [Header("Components")]
    [Range(1, 50)][SerializeField] float grappleSpeed = 25f;
    [SerializeField] GameObject hook;
    public bool canGrapple = true;
    Vector2 shootDirection;
    Rigidbody2D rb2D;
    Grapple hookCS;

    [Header("Inputs")]
    PlayerInputs playerControls;
    InputAction grappleAction;
    InputAction jumpAction;
    Vector3 mousePos;
    [Range(25, 75)][SerializeField] float jumpForce = 50;


    private void Awake()
    {
        playerControls = new PlayerInputs();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        grappleAction = playerControls.Player.Grapple;
        grappleAction.Enable();
        grappleAction.performed += ShootGrapple;

        jumpAction = playerControls.Player.Jump;
        jumpAction.Enable();
        jumpAction.performed += CancelGrapple;

        hookCS = hook.GetComponent<Grapple>();
    }

    private void OnDisable()
    {
        grappleAction.Disable();
        jumpAction.Disable();
    }
    void ShootGrapple(InputAction.CallbackContext context)
    {
        if (canGrapple)
        {
            canGrapple = false;
            hook.transform.parent = null;
            hook.SetActive(true);

            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.transform.position.z;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x -= objectPos.x;
            mousePos.y -= objectPos.y;

            shootDirection = (mousePos - hook.transform.position).normalized;

            hookCS.grappleSpeed = grappleSpeed;
            hookCS.StartCoroutine(hookCS.GrappleHookStick());
            hook.GetComponent<Rigidbody2D>().velocity = shootDirection * grappleSpeed;
        }
        else
        {
            hookCS.SetParent();
        }
    }

    void CancelGrapple(InputAction.CallbackContext context)
    {
        if (!canGrapple || !)
        {
            rb2D.AddForce(new(0, jumpForce * 10)); //move speed fwd
            hookCS.SetParent();
        }
    }
}
