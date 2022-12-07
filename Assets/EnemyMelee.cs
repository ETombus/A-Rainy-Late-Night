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
            if (Vector3.Distance(handler.playerTrans.position, transform.position) > punchingDistance)
                handler.movement.MoveEnemy(handler.playerTrans.position);
            else if (Vector3.Distance(handler.playerTrans.position, transform.position) < punchingDistance)
            {
                punchingTimer += Time.deltaTime;

                if (punchingTimer >= punchingCooldown)
                {
                    handler.playerHealth.ReduceHealth(10);
                    punchingTimer = 0;
                    Debug.Log("Punching Player");
                }
            }
        }
    }
}
