using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : HealthHandler
{
    [Header("Healthbar")]
    [SerializeField] public Slider healthBar;
    [SerializeField] private AnimationCurve healthShown;

    private void Start()
    {
        if (healthBar != null)
            healthBar.maxValue = maxHealth;
        health = maxHealth;
    }

    public void ReduceHealth(float reduceValue)
    {
        //Debug.Log("hp: "+healthBar.value);
        health -= reduceValue;
        if (healthBar != null)
            healthBar.value = healthShown.Evaluate(health / 100) * 100;
    }

    public void AddHealth()
    {
        if (healthBar != null)
            healthBar.value = health;
    }
}
