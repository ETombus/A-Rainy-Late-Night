using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float damageFrequency;
    [SerializeField] float damageTimer;
    bool isInWater;

    Healthbar health;

    private void Update()
    {

        if (isInWater)
        {
            if (damageTimer < damageFrequency) { damageTimer += Time.deltaTime; }

            if (damageTimer >= damageFrequency)
            {
                Debug.Log("Should Have reduced health");
                health.ReduceHealth(damage, true);
                damageTimer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            health = collision.gameObject.GetComponent<Healthbar>();
            isInWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { isInWater = false; }
    }
}
