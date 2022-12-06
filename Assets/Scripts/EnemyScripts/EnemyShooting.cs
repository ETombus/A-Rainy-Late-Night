using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public bool isShooting;
    [SerializeField] float shootCooldown = 0.5f;
    float shootTimer;
    bool canShoot;

    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed = 50f;
    EnemyHandler handler;

    private void Start()
    {
        handler = GetComponent<EnemyHandler>();
        shootTimer = shootCooldown;
    }

    private void Update()
    {
        if (isShooting)
        {
            if (shootTimer > 0)
                shootTimer -= Time.deltaTime;

            else if(shootTimer <= 0)
            {
                Shoot();
                shootTimer = shootCooldown;
            }
        }
    }

    public void Shoot()
    {
        GameObject bulletInstance = Instantiate(bullet, transform.position, transform.rotation);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = GetShootVector().normalized * bulletSpeed;
    }

    private Vector2 GetShootVector()
    {
        Vector2 targetVector = handler.player.transform.position - transform.position;
        return targetVector;
    }
}
