using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    private Healthbar playerHealth;

    public float damageInterval = 0.2f;
    public float damageAmmount = 1;
    private bool dealDamage = false;

    float timer = 0;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (dealDamage)
        {
            timer = damageInterval;
            playerHealth.ReduceHealth(damageAmmount, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Healthbar>();
            dealDamage = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dealDamage = false;
        }
    }


}
