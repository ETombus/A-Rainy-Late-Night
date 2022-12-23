using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    //Inputs
    PlayerInputs playerControls;
    private InputAction aim;
    private InputAction fire;

    bool isAiming = false;
    [SerializeField] GameObject bullet;

    private void Awake() { playerControls = new PlayerInputs(); }

    private void OnEnable()
    {
        //aim = playerControls.Player.Aim;
        fire = playerControls.Player.SlashShoot;
        aim.Enable();
        fire.Enable();

        aim.performed += Aiming;
        fire.performed += Shoot;
    }

    private void OnDisable()
    {
        aim.Disable();
        fire.Disable();
    }

    private void Aiming(InputAction.CallbackContext context)
    {
        isAiming = !isAiming;
        Debug.Log("Aiming is" + isAiming);
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (isAiming)
        {
            Debug.Log("shooting");
        }
    }
}
