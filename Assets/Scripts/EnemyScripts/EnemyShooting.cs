using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform gunTrans; //transform to shoot from
    [SerializeField] GameObject bullet;

    [Header("Scripts")]
    private EnemyHandler handler;
    private EnemySpineController spineController;

    [Header("Values")]
    [Range(1f, 10f)][SerializeField] float minDistanceToPlayer;
    public int damage = 10;
    [Tooltip("bullets shot before reload")]
    [SerializeField] int magSize = 5;
    [Tooltip("bullets travel speed")]
    [SerializeField] float bulletSpeed = 50f;
    [Tooltip("delay between bullets")]
    [SerializeField] float shootCooldown = 0.1f;
    [Tooltip("read the name, you don't need a tooltip")]
    [SerializeField] float reloadTime = 1f;
    private bool firing = false;

    private void Start()
    {
        handler = GetComponent<EnemyHandler>();
        spineController = GetComponentInChildren<EnemySpineController>();
    }

    private void Update()
    {
        if (handler.currentMode == EnemyHandler.Mode.Aggression && !firing)
        {
            Vector2 direction = transform.position - handler.playerTrans.position;
            float targetPos = handler.playerTrans.position.x + direction.normalized.x * minDistanceToPlayer;

            if (Mathf.Abs(handler.playerTrans.position.x - transform.position.x) < minDistanceToPlayer)
                handler.movement.MoveEnemy(new(targetPos, transform.position.y));
            else
                handler.movement.StopEnemy();

            spineController.PlayAttackAnimation();
        }
    }

    public IEnumerator Shoot()
    {
        firing = true;

        for (int i = magSize; i > 0; i--)
        {
            spineController.PlayAttackSound();

            GameObject bulletInstance = Instantiate(bullet, gunTrans.position, transform.rotation);
            bulletInstance.GetComponent<BulletScript>().damage = damage;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = GetShootVector().normalized * bulletSpeed;
            Destroy(bulletInstance, 2f);

            yield return new WaitForSeconds(shootCooldown);
        }

        //reload
        yield return new WaitForSeconds(reloadTime);

        firing = false;
    }

    private Vector2 GetShootVector()
    {
        Vector2 targetVector = handler.playerTrans.position - transform.position;
        return targetVector;
    }
}
