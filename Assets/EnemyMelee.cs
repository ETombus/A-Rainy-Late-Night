using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    EnemyHandler handler;
    EnemyHandler.Mode previousMode;
    private void Start()
    {
        handler = GetComponent<EnemyHandler>();
    }

    private void Update()
    {
        if (handler.currentMode == EnemyHandler.Mode.Aggression)
        {
            handler.movement.MoveEnemy(handler.player.transform.position);
        }
    }

}
