using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class Healthbar : HealthHandler
{
    [Header("Healthbar")]
    [SerializeField] public Slider healthBar;
    [SerializeField] private AnimationCurve healthShown;

    [Header("Sound")]
    [SerializeField] PlayerSoundHandler soundHandler;
    [SerializeField] AudioClip[] clips;

    [Header("Components")]
    [SerializeField] ParticleSystem damageSpark;

    GameOverManager gameOver;

    private void Start()
    {
        soundHandler = GetComponent<PlayerSoundHandler>();
        gameOver = GameObject.Find("GameManager").GetComponent<GameOverManager>();

        if (healthBar != null)
            healthBar.maxValue = maxHealth;
        health = maxHealth;
    }

    public void ReduceHealth(float reduceValue, bool rainDamage)
    {
        if(!rainDamage)
            damageSpark.Play();

        if (health - reduceValue > 0)
        {
            health -= reduceValue;
            if (healthBar != null)
                healthBar.value = healthShown.Evaluate(health / 100) * 100;

            if (clips.Length > 0)
                soundHandler.PlaySound(clips[Random.Range(0, clips.Length)], 0.2f);
        }
        else { PlayerDeath(); }

    }

    public void AddHealth()
    {
        if (healthBar != null)
            healthBar.value = health;
    }

    void PlayerDeath()
    {
        gameOver.PlayerDeath();
        health = maxHealth;
    }
}
