using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    //public bool isShooting;
    [SerializeField] float shootCooldown = 0.5f;
    [SerializeField] Transform gunTrans; //transform to shoot from
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
        if (handler.currentMode == EnemyHandler.Mode.Aggression)
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
        GameObject bulletInstance = Instantiate(bullet, gunTrans.position, transform.rotation);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = GetShootVector().normalized * bulletSpeed;
        Destroy(bulletInstance, 2f);
    }

    private Vector2 GetShootVector()
    {
        Vector2 targetVector = handler.playerTrans.position - transform.position;
        return targetVector;
    }
}
