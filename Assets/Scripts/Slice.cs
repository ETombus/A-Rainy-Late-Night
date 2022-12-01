using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slice : MonoBehaviour
{
    //Inputs
    PlayerInputs playerControls;
    private InputAction slice;

    [SerializeField] GameObject sliceHitbox;
    [SerializeField] float attackDuration = 0.2f;
    [SerializeField] float attackCooldown = 0.2f;
    public bool canAttack;
    private float attackTimer;
    private float attackCooldownTimer;

    //UmbrellaOpener umbrella;

    private void Awake()
    {
        playerControls = new PlayerInputs();
        //umbrella = GetComponentInChildren<UmbrellaOpener>();
    }

    private void OnEnable()
    {
        slice = playerControls.Player.Fire;
        slice.Enable();
        slice.performed += StandardSlice;
    }

    private void OnDisable()
    {
        slice.Disable();
    }

    private void StandardSlice(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            //Debug.Log("We Sliced");
            sliceHitbox.SetActive(true);
            attackTimer = attackDuration;
            canAttack = false;
            attackCooldownTimer = attackCooldown;

            //umbrella.umbrellaOverrideBool = true;
            //umbrella.CloseUmbrella();
        }
    }

    private void Update()
    {
        if (attackTimer > 0) { attackTimer -= Time.deltaTime; }
        else if (attackTimer <= 0)
        {
            sliceHitbox.SetActive(false);
            //umbrella.umbrellaOverrideBool = false;
        }

        if (attackCooldownTimer > 0) { attackCooldownTimer -= Time.deltaTime; }
        else if (attackCooldownTimer <= 0) { canAttack = true; }
    }
}
