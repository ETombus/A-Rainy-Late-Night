using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthHandler targetHealth = collision.GetComponent<HealthHandler>();
        if (targetHealth == null) { return; }

        targetHealth.ReduceHealth(damage);
    }
}
