using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Healthbar : HealthHandler
{
    [Header("Healthbar")]
    [SerializeField] public Slider healthBar;
    [SerializeField] private AnimationCurve healthShown;

    [Header("Sound")]
    [SerializeField] PlayerSoundHandler soundHandler;
    [SerializeField] AudioClip rainDamageSound;

    [Header("Components")]
    [SerializeField] ParticleSystem damageSpark;
    GameOverManager gameOver;
    HookScript hook;
    PlayerStateHandler state;
    Rigidbody2D rbody;

    [Header("Color")]
    [SerializeField] Color fullHealthColor;
    [SerializeField] Color halfHealthColor;
    [SerializeField] Color lowHealthColor;

    float loopTimer;

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
        if (!rainDamage)
            damageSpark.Play();

        if (health - reduceValue > 0)
        {
            health -= reduceValue;
            if (healthBar != null)
                healthBar.value = healthShown.Evaluate(health / 100) * 100;

            if (health >= maxHealth / 2) { healthBar.GetComponentInChildren<Image>().color = fullHealthColor; }
            else if (health <= maxHealth / 5) { healthBar.GetComponentInChildren<Image>().color = lowHealthColor; }
            else if (health <= maxHealth / 2) { healthBar.GetComponentInChildren<Image>().color = halfHealthColor; }
        }
        else { PlayerDeath(); }

    }

    public void AddHealth(float addedHealth, bool updateHealthbar)
    {
        AddHealth(addedHealth);

        if (healthBar != null)
            healthBar.value = health;
    }

    public void PlayerDeath()
    {
        try
        {
            hook = FindObjectOfType<HookScript>();
            hook.ResetHook();
        }
        catch (System.Exception)
        {  }

        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        gameOver.PlayerDeath();
        health = maxHealth;
        GetComponentInChildren<RifleScript>().AddAmmo(10);

        rbody.velocity = Vector2.zero;
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        state = GetComponent<PlayerStateHandler>();
        state.currentMoveState = PlayerStateHandler.MovementStates.Idle;
    }
}
