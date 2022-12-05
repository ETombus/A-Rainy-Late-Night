using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : HealthHandler
{
    [Header("Healthbar")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private AnimationCurve healthShown;

    private void Start()
    {
        healthBar.maxValue = maxHealth;
        health = maxHealth;
    }

    public void ReduceHealth(int reduceValue)
    {
        health -= reduceValue;
        healthBar.value = healthShown.Evaluate(health / 100) * 100;
    }

    public void AddHealth()
    {
        healthBar.value = health;
    }
}
