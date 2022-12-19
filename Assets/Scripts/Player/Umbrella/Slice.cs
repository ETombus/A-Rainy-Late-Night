using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slice : MonoBehaviour
{
    [SerializeField] GameObject sliceHitbox;
    [SerializeField] float attackDuration = 0.2f;
    [SerializeField] float attackCooldown = 0.2f;
    public bool canAttack;
    private float attackTimer;
    private float attackCooldownTimer;

    public bool isSlicing = false;
    public float sliceDirection;

    //UmbrellaOpener umbrella;

    public void StandardSlice()
    {
        if (canAttack)
        {
            //Debug.Log("We Sliced");
            sliceHitbox.SetActive(true);
            attackTimer = attackDuration;
            canAttack = false;
            attackCooldownTimer = attackCooldown;

            isSlicing = true;
            StopCoroutine(slashDuration());
            StartCoroutine(slashDuration());


            Vector2 mousePos =  Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
            sliceDirection = Mathf.RoundToInt(mousePos.normalized.x);


            //umbrella.umbrellaOverrideBool = true;
            //umbrella.CloseUmbrella();
        }
    }

    IEnumerator slashDuration()
    {
        yield return new WaitForSeconds(0.1f);
        isSlicing = false;
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
