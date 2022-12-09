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
            if (Mathf.Abs(handler.playerTrans.position.x - transform.position.x) > punchingDistance)
                handler.movement.MoveEnemy(handler.playerTrans.position);

            else if (Mathf.Abs(handler.playerTrans.position.x - transform.position.x) <= punchingDistance)
            {
                handler.movement.StopEnemy();
                punchingTimer += Time.deltaTime;

                if (punchingTimer >= punchingCooldown)
                {
                    handler.playerHealth.ReduceHealth(10);
                    punchingTimer = 0;
                    handler.PlaySound(handler.thisType);
                    enemySpine.PlayAttackAnimation();
                    //Debug.Log("Punching Player");
                }
            }
        }
    }
}
