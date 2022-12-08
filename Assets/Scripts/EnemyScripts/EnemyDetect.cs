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
    [SerializeField] LayerMask groundLayers;

    [SerializeField] float seachDuration = 3f;
    float searchTimer;

    [SerializeField] Sprite[] markers; //0 = ?, 1 = !, 2 = anger
    SpriteRenderer markerRenderer;

    EnemyHandler handler;

    private void Start()
    {
        handler = GetComponent<EnemyHandler>();
        markerRenderer = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        markerRenderer.sprite = null;
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
        markerRenderer.sprite = null;
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
        else { detectedPlayer = false; }
    }

    void SearchForPlayerWithinRange()
    {
        markerRenderer.sprite = markers[2];
        handler.FlipRotation(handler.playerTrans.position.x - transform.position.x);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, handler.playerTrans.position - transform.position,
            Vector2.Distance(transform.position, handler.playerTrans.position), detectableLayers);
        if (hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.point);
            Debug.Log(hit.collider.gameObject.layer);

            if (Vector2.Distance(transform.position, handler.playerTrans.position) > detectionDistance
               || ((1 << hit.collider.gameObject.layer) & groundLayers) != 0) //compare the objects layer with the layermask
            {
                seesPlayer = false;
                handler.currentMode = EnemyHandler.Mode.Search;
                lastSeenPlayerLocation = handler.playerTrans.position;
            }
        }
    }

    void SearchForPlayerOutofRange()
    {
        searchTimer += Time.deltaTime;
        markerRenderer.sprite = markers[0];

        RaycastHit2D hit = Physics2D.Raycast(transform.position, handler.playerTrans.position - transform.position,
            Vector2.Distance(transform.position, handler.playerTrans.position), groundLayers);

        Debug.DrawLine(transform.position, handler.playerTrans.position, Color.blue);

        if (Vector2.Distance(transform.position, handler.playerTrans.position) < detectionDistance
            && hit.collider == null)
        {
            handler.currentMode = EnemyHandler.Mode.Aggression;
        }

        if (searchTimer >= seachDuration)
        {
            handler.currentMode = EnemyHandler.Mode.Patrol;
            handler.FlipRotation();
            searchTimer = 0;
        }
    }
}
