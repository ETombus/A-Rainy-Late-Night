using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHookShoot : MonoBehaviour
{
    [SerializeField] GameObject hook;
    PlayerInputs playerControls;
    InputAction grapple;
    float grappleSpeed = 2.5f;
    bool canGrapple = true;

    private void Awake() { playerControls = new PlayerInputs(); }

    private void OnEnable()
    {
        grapple = playerControls.Player.Grapple;
        grapple.Enable();
        grapple.performed += ShootGrapple;
    }

    void ShootGrapple(InputAction.CallbackContext context)
    {
        if(canGrapple)
        {
            canGrapple=false;
            hook.transform.parent = null;
            hook.SetActive(true);

            Vector3 mousePos = Mouse.current.position.ReadValue();   
            mousePos.z=Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);

            var hookCS = hook.GetComponent<Grapple>();
            hookCS.grappleSpeed = grappleSpeed;
            hookCS.StartCoroutine(hookCS.GrappleHookStick());
            hook.GetComponent<Rigidbody2D>().velocity = (Worldpos-hook.transform.position) * grappleSpeed;
        }

    }
}
