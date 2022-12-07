using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    [SerializeField] float detectionDistance = 10;

    public Vector2 lastSeenPlayerLocation;

    public bool seesPlayer;
    [SerializeField] bool detectedPlayer;

    [SerializeField] LayerMask detectableLayers;

    [SerializeField] float seachDuration = 3f;
    float searchTimer;

    EnemyHandler handler;

    private void Start()
    {
        handler = GetComponentInParent<EnemyHandler>();
    }

    private void Update()
    {
        switch (handler.currentMode)
        {
            case EnemyHandler.Mode.Patrol:
                SendDetectionRay();
                break;
            case EnemyHandler.Mode.Aggression:
                SearchForPlayerWithinRange();
                break;
            case EnemyHandler.Mode.Search:
                SearchForPlayerOutofRange();
                break;
            case EnemyHandler.Mode.Idle:
                SendDetectionRay();
                break;
            default:
                break;
        }
    }

    private void SendDetectionRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, detectionDistance, detectableLayers);

        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                detectedPlayer = false;
                //Debug.Log(transform.parent.gameObject.name + " hit ground object " + hit.collider.gameObject.name);
            }
            else if (hit.collider.gameObject.CompareTag("Player"))
            {
                detectedPlayer = true;
                seesPlayer = true;
                //Debug.Log(transform.parent.gameObject.name + " hit player object " + hit.collider.gameObject.name);
                handler.currentMode = EnemyHandler.Mode.Aggression;
            }
        }
        else
        {
            detectedPlayer = false;
        }
    }

    void SearchForPlayerWithinRange()
    {
        handler.FlipRotation(handler.playerTrans.position.x - transform.position.x);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, handler.playerTrans.position - transform.position,
            Vector2.Distance(transform.position, handler.playerTrans.position), detectableLayers);
        Debug.DrawLine(transform.position, handler.playerTrans.position);

        if (Vector2.Distance(transform.position, handler.playerTrans.position) > detectionDistance)
        {
            seesPlayer = false;
            handler.currentMode = EnemyHandler.Mode.Search;
            lastSeenPlayerLocation = handler.playerTrans.position;
        }
    }

    void SearchForPlayerOutofRange()
    {
        searchTimer += Time.deltaTime;


        if (Vector2.Distance(transform.position, handler.playerTrans.position) < detectionDistance)
        {
            handler.currentMode = EnemyHandler.Mode.Aggression;
        }

        if(searchTimer >= seachDuration)
        {
            handler.currentMode = EnemyHandler.Mode.Patrol;
            handler.FlipRotation();
            searchTimer = 0;
        }
    }
}
