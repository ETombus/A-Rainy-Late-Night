using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector2[] movePoints;

    public float acceleration;
    public float decceleration;
    public float maxWalkingSpeed = 3;
    public float maxRunningSpeed = 5;

    private float maxSpeed;
    public float waitTime;

    public float targetOffsetAmmount = 1;

    private Rigidbody2D rigBody;
    public int moveIndex = 0;

    EnemyHandler handler;

    private void Start()
    {
        rigBody = GetComponent<Rigidbody2D>();
        handler = GetComponent<EnemyHandler>();

        maxSpeed = maxWalkingSpeed;

        for (int i = 0; i < movePoints.Length; i++)
        {
            movePoints[i] += (Vector2)transform.position;
        }
    }

    private void FixedUpdate()
    {
        handler.isMoving = rigBody.velocity.x != 0 ? true : false;

        if (!handler.edgeDetection.DetectEdges())
            StopEnemy();

        switch (handler.currentMode)
        {
            case EnemyHandler.Mode.Patrol:
                PatrolMode();
                maxSpeed = maxWalkingSpeed;
                break;
            case EnemyHandler.Mode.Aggression:
                maxSpeed = maxRunningSpeed;
                break;
            case EnemyHandler.Mode.Search:
                maxSpeed = maxRunningSpeed;
                SearchForPlayer();
                break;
            case EnemyHandler.Mode.Idle:
                maxSpeed = maxWalkingSpeed;
                PatrolMode();
                break;
            case EnemyHandler.Mode.Dead:
                StopEnemy();
                break;
            default:
                break;
        }
    }

    bool idle = false;
    void PatrolMode()
    {
        if (transform.position.x - movePoints[moveIndex].x < targetOffsetAmmount && transform.position.x - movePoints[moveIndex].x > -targetOffsetAmmount && !idle)
        {
            handler.currentMode = EnemyHandler.Mode.Idle;
            idle = true;
            StartCoroutine(WaitBetweenPatrol(1));
        }
        else if (idle)
        {
            StopEnemy();
        }
        else
        {
            handler.currentMode = EnemyHandler.Mode.Patrol;
            MoveEnemy(movePoints[moveIndex]);
        }
    }

   public void SearchForPlayer()
    {
        if (handler.edgeDetection.DetectEdges())
            MoveEnemy(handler.detection.lastSeenPlayerLocation);
    }

    public void MoveEnemy(Vector2 targetPos)
    {
        if (!handler.edgeDetection.DetectEdges()) { return; }

        rigBody.AddForce(new Vector2(targetPos.x - transform.position.x, 0).normalized * acceleration);

        if (rigBody.velocity.magnitude > maxSpeed)
            rigBody.velocity = Vector2.ClampMagnitude(rigBody.velocity, maxSpeed);
    }

    public void StopEnemy()
    {
        rigBody.velocity *= decceleration;
    }

    IEnumerator WaitBetweenPatrol(float idleTime)
    {
        yield return new WaitForSeconds(idleTime);

        if (moveIndex == movePoints.Length - 1)
            moveIndex = 0;
        else
            moveIndex++;

        handler.FlipRotation(movePoints[moveIndex].x - transform.position.x);

        idle = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        //Changes it due to the movePoints getting updated when start playes

        if (!Application.isPlaying)
        {
            for (int i = 0; i < movePoints.Length; i++)
            {
                Gizmos.DrawWireSphere(movePoints[i] + (Vector2)transform.position, 0.5f);
            }
        }
        else
        {
            for (int x = 0; x < movePoints.Length; x++)
            {
                Gizmos.DrawWireSphere(movePoints[x], 0.5f);
            }
        }
    }
}
