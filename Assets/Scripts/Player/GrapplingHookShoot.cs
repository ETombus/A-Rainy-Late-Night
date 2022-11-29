using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHookShoot : MonoBehaviour
{
    [Header("Components")]
    public bool canGrapple = true;
    [SerializeField] GameObject hook;
    [Range(50, 100)][SerializeField] float grappleSpeed = 65f;
    PlayerInputs playerControls;
    InputAction grappleAction;
    Grapple hookCS;


    private void Awake() { playerControls = new PlayerInputs(); }

    private void OnEnable()
    {
        grappleAction = playerControls.Player.Grapple;
        grappleAction.Enable();
        grappleAction.performed += ShootGrapple;
        hookCS = hook.GetComponent<Grapple>();
    }

    void ShootGrapple(InputAction.CallbackContext context)
    {
        Debug.Log(canGrapple);
        if(canGrapple)
        {
            canGrapple=false;
            hook.transform.parent = null;
            hook.SetActive(true);

            Vector3 mousePos = Mouse.current.position.ReadValue();   
            mousePos.z=Camera.main.nearClipPlane;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);

            hookCS.grappleSpeed = grappleSpeed;
            hookCS.StartCoroutine(hookCS.GrappleHookStick());
            hook.GetComponent<Rigidbody2D>().velocity = (Worldpos-hook.transform.position).normalized * grappleSpeed;
        }
        else
        {
            hookCS.SetParent();
        }

    }
}
