using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 50;
    [SerializeField] UmbrellaStateHandler umbrella;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthHandler targetHealth = collision.GetComponent<HealthHandler>();

        collision.transform.SendMessage(nameof(InteractScript.Hit), SendMessageOptions.DontRequireReceiver);

        if(collision.CompareTag("Enemy"))
            umbrella.soundHandler.PlaySound(umbrella.clips[6]);

        if (targetHealth == null) { return; }

        targetHealth.ReduceHealth(damage);
    }
}
