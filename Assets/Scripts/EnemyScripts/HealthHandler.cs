using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    public float health;
    public int maxHealth = 100;

    public UnityEvent damageTrigger, deathTrigger;

    [SerializeField] GameObject bloodParticles;
    [SerializeField] GameObject hookPoint;
    [SerializeField] GameObject marker;

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
            {
                var damageParticle = Instantiate(bloodParticles, transform.position, transform.rotation);
                Destroy(damageParticle.gameObject, 1);
            }

            //Only invokes if there is anything there
            damageTrigger.Invoke();

            EnemyHandler handler = GetComponent<EnemyHandler>();
            if (handler != null) { handler.AlertEnemy(); }
        }
        else
            Death();
    }

    public void AddHealth(float addedHealth)
    {
        if (health + addedHealth <= 100)
            health += addedHealth;
        else if (health + addedHealth > 100)
            health = 100;
    }

    void Death()
    {
        //Add code for death
        if (marker != null)
            marker.SetActive(false);

        if (hookPoint != null)
            Destroy(hookPoint);

        if (bloodParticles != null)
            Instantiate(bloodParticles, transform.position, transform.rotation);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        GameObject.FindWithTag("Player").GetComponentInChildren<RifleScript>().AddAmmo(1);

        if (gameObject != null && deathTrigger.GetPersistentEventCount() == 0)
        {
            Destroy(gameObject);

        }
        else
        {
            deathTrigger.Invoke();
        }
    }
}
