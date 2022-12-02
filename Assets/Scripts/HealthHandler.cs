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
            Instantiate(bloodParticles, transform.position, transform.rotation);
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

        //Debug.Log(gameObject.name + " Died");

        Instantiate(bloodParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
