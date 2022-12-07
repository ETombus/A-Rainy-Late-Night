using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    public float health;
    public int maxHealth = 100;

    [SerializeField] GameObject bloodParticles;

    private void Start()
    {
        health = maxHealth;
    }

    public void ReduceHealth(float reducedHealth)
    {
        if (health - reducedHealth > 0)
        {
            health -= reducedHealth;
            if (bloodParticles != null)
                Instantiate(bloodParticles, transform.position, transform.rotation);

            EnemyHandler handler = GetComponent<EnemyHandler>();
            if(handler != null) { handler.AlertEnemy(); }
        }

        else
            Death();
    }

    public void AddHealth(float addedHealth)
    {
        health += addedHealth;
    }

    void Death()
    {
        //Add code for death

        if (bloodParticles != null)
            Instantiate(bloodParticles, transform.position, transform.rotation);

        if (gameObject != null)
            Destroy(gameObject);
    }
}
