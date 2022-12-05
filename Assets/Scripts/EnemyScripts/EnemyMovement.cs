using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector2[] movePoints;

    public float acceleration;
    public float maxSpeed;
    public float decceleration;

    public float waitTime;

    public float targetOffsetAmmount = 1;

    private Rigidbody2D rigBody;
    public int moveIndex = 0;

    EnemyHandler handler;

    private void Start()
    {
        rigBody = GetComponent<Rigidbody2D>();
        handler = GetComponent<EnemyHandler>();

        for (int i = 0; i < movePoints.Length; i++)
        {
            movePoints[i] += (Vector2)transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (!handler.edgeDetection.DetectEdges())
            rigBody.velocity *= decceleration;

        if (handler.currentMode == EnemyHandler.Mode.Patrol)
            PatrolMode();
        else if (handler.currentMode == EnemyHandler.Mode.Search)
            SearchForPlayer();
    }

    bool idle = false;
    void PatrolMode()
    {
        if (transform.position.x - movePoints[moveIndex].x < targetOffsetAmmount && transform.position.x - movePoints[moveIndex].x > -targetOffsetAmmount && !idle)
        {
            idle = true;
            StartCoroutine(waitBetweenPatrol(1));
        }
        else if (idle)
        {
            rigBody.velocity *= decceleration;
        }
        else
        {
            MoveEnemy(movePoints[moveIndex]);
        }
    }

    void SearchForPlayer()
    {
        if (handler.edgeDetection.DetectEdges())
            MoveEnemy(handler.detection.lastSeenPlayerLocation);
    }

    void MoveEnemy(Vector2 targetPos)
    {
        rigBody.AddForce(new Vector2(targetPos.x - transform.position.x, 0).normalized * acceleration);

        if (rigBody.velocity.magnitude > maxSpeed)
            rigBody.velocity = Vector2.ClampMagnitude(rigBody.velocity, maxSpeed);
    }

    IEnumerator waitBetweenPatrol(float idleTime)
    {
        yield return new WaitForSeconds(idleTime);

        if (moveIndex == movePoints.Length - 1)
            moveIndex = 0;
        else
            moveIndex++;

        FlipRotation(movePoints[moveIndex].x - transform.position.x);

        idle = false;
    }

    private void FlipRotation(float direction)
    {
        if (direction < 0)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        else
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    }
    public void FlipRotationPublic()
    {
        FlipRotation(movePoints[moveIndex].x - transform.position.x);
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
