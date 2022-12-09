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

    private void Start()
    {
        handler = GetComponent<EnemyHandler>();
    }

    private void Update()
    {
        if (handler.currentMode == EnemyHandler.Mode.Aggression)
        {
            if (Mathf.Abs(handler.playerTrans.position.x - transform.position.x) > punchingDistance)
                handler.movement.MoveEnemy(handler.playerTrans.position);

            //if (Vector2.Distance(handler.playerTrans.position, transform.position) > punchingDistance)
            //{
            //    handler.movement.MoveEnemy(handler.playerTrans.position);
            //    Debug.Log("Punching distance is " + punchingDistance);
            //}

            else if (Mathf.Abs(handler.playerTrans.position.x - transform.position.x) <= punchingDistance)
            {
                handler.movement.StopEnemy();
                punchingTimer += Time.deltaTime;

                if (punchingTimer >= punchingCooldown)
                {
                    handler.playerHealth.ReduceHealth(10);
                    punchingTimer = 0;
                    handler.PlaySound(handler.thisType);
                    //Debug.Log("Punching Player");
                }
            }
        }
    }
}
