using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    EnemyHandler handler;

    [SerializeField] float punchingDistance = 0.5f;

    [SerializeField] float punchingCooldown = 0.6f;
    float punchingTimer;

    [SerializeField] int damage;
    private EnemySpineController enemySpine;


    private void Start()
    {
        handler = GetComponent<EnemyHandler>();
        enemySpine = GetComponentInChildren<EnemySpineController>();
    }

    private void Update()
    {
        if (handler.currentMode == EnemyHandler.Mode.Aggression)
        {
            if (XDist() > punchingDistance)
                handler.movement.MoveEnemy(handler.playerTrans.position);
            else if (YDist() <= punchingDistance)
            {
                handler.movement.StopEnemy();
                punchingTimer += Time.deltaTime;

                if (punchingTimer >= punchingCooldown)
                {
                    punchingTimer = 0;
                    enemySpine.PlayAttackAnimation();
                    //Debug.Log("Punching Player");
                }
            }
        }
    }

    private float XDist() { return Mathf.Abs(handler.playerTrans.position.x - transform.position.x); }
    private float YDist() { return Mathf.Abs(handler.playerTrans.position.x - transform.position.x); }

    public void Attack()
    {
        if (XDist() <= punchingDistance && YDist() <= punchingDistance)
        {
            handler.playerHealth.ReduceHealth(10, false);
            handler.player.GetComponent<Walking>().Knockback(10, transform.position);
            handler.PlaySound(handler.thisType);
        }
        else
        {
            //whiff sound effect
        }
    }
}
