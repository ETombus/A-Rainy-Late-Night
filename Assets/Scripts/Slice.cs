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
    private float attackTimer;

    private void Awake()
    {
        playerControls = new PlayerInputs();
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
        Debug.Log("We Sliced");
        sliceHitbox.SetActive(true);
        attackTimer = attackDuration;
    }

    private void Update()
    {
        if (attackTimer > 0) { attackTimer -= Time.deltaTime; }
        else if (attackTimer <= 0) { sliceHitbox.SetActive(false); }
    }
}
