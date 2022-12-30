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

    [SerializeField] AudioClip detectSound;

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
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, transform.right + (transform.up / 2), detectionDistance, detectableLayers);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, transform.right + (-transform.up / 2), detectionDistance, detectableLayers);


        //RaycastHit2D boxHit = Physics2D.BoxCast(transform.position,
        //    new(detectionDistance, detectionDistance), 0f, transform.right, detectionDistance, detectableLayers);


        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("Player")) { DetectPlayer(); }
            else { detectedPlayer = false; }
        }
        if (hitUp)
        {
            if (hitUp.collider.gameObject.CompareTag("Player")) { DetectPlayer(); }
            else { detectedPlayer = false; }

        }
        if (hitDown)
        {
            if (hitDown.collider.gameObject.CompareTag("Player")) { DetectPlayer(); }
            else { detectedPlayer = false; }
        }
        else { detectedPlayer = false; }
    }

    void DetectPlayer()
    {
        detectedPlayer = true;
        seesPlayer = true;
        handler.currentMode = EnemyHandler.Mode.Aggression;

        if(detectSound!= null) 
            GetComponent<AudioSource>().PlayOneShot(detectSound);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.right * detectionDistance);
        Gizmos.DrawRay(transform.position, (transform.right + (transform.up / 2)) * detectionDistance);
        Gizmos.DrawRay(transform.position, (transform.right + (-transform.up / 2)) * detectionDistance);
    }
}
