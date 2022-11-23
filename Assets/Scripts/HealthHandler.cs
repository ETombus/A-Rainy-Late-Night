using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private Slider healthBar;
    [SerializeField] private AnimationCurve healthShown;

    private int maxHealth = 100;

    private void Start()
    {
        healthBar.maxValue = maxHealth;
        health = maxHealth;
    }

    public void ReduceHealth(float reducedHealth)
    {
        health -= reducedHealth;

        healthBar.value = healthShown.Evaluate(health / 100) * 100;
    }

    public void AddHealth(float addedHealth)
    {
        health += addedHealth;

        healthBar.value = health;
    }
}
