using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class GrappleInput : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform ropeStart;
    [SerializeField] LayerMask rayIgnore;
    [HideInInspector] public List<GameObject> hookPoints;
    UmbrellaStateHandler umbrella;
    PlayerStateHandler playerState;
    GameObject targetPoint;
    string hookPointTag = "HookPoint";

    [Header("Inputs")]
    PlayerInputs playerControls;
    InputAction grappleAction;
    Vector2 mousePos;
    Vector2 worldPos;

    [Header("Values")]
    [Range(0, 25)][SerializeField] float hookMaxReach;
    [Range(0, 25)][SerializeField] float maxMouseDistance;
    [SerializeField] float minHookDistance;
    [SerializeField] float hookSpeed;
    [SerializeField] float playerSpeed;
    [SerializeField] float playerAcceleration;
    [SerializeField] AnimationCurve playerSpeedOverTime;
    float closestHookDistance;

    public bool canGrapple = true;
    public bool targetLocked = false;

    public bool showGrappleLeangth = false;

    private void Start()
    {
        umbrella = GetComponentInChildren<UmbrellaStateHandler>();
        playerState = GetComponent<PlayerStateHandler>();
    }

    private void Awake()
    {
        hookPoints.AddRange(GameObject.FindGameObjectsWithTag(hookPointTag));

        try
        {
            targetPoint = hookPoints[0];
        }
        catch { }
        closestHookDistance = maxMouseDistance;
    }

    private void FixedUpdate()
    {
        mousePos = Mouse.current.position.ReadValue();
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetLocked = false;
        closestHookDistance = maxMouseDistance;

        foreach (GameObject point in hookPoints)
        {
            if (point == null)
            {
                hookPoints.Remove(point);
                break;
            }
            else
            {
                float distance = Vector2.SqrMagnitude((Vector2)point.transform.position - worldPos);

                if (distance <= closestHookDistance && RayHitPlayer(point))
                {
                    targetLocked = true;
                    targetPoint = point;
                    closestHookDistance = distance;

                    // Visual for hovering 
                    point.GetComponentInChildren<DetectedByCamera>().isSelected = true;
                    point.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else if (distance > maxMouseDistance || !RayHitPlayer(point))
                {

                    // Visual for non-hovering 
                    point.GetComponentInChildren<DetectedByCamera>().isSelected = false;
                    point.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

    private bool RayHitPlayer(GameObject target)
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = target.transform.position;
        Collider2D rayTarget = Physics2D.Raycast(targetPos, (pos - targetPos).normalized, hookMaxReach, rayIgnore).collider;

        if (rayTarget == GetComponent<Collider2D>())
        { return true; }
        else
        { return false; }
    }

    public void ShootGrapple()
    {
        float distanceToMouse = Vector2.SqrMagnitude((Vector2)targetPoint.transform.position - worldPos);
        float distanceToPlayer = Vector2.SqrMagnitude((Vector2)targetPoint.transform.position - (Vector2)transform.position);

        if ((targetPoint.transform.position.x - transform.position.x) > 0)
        {
            playerState.facingRight = true;
        }
        else if ((targetPoint.transform.position.x - transform.position.x) < 0)
        {
            playerState.facingRight = false;
        }

        if (distanceToMouse <= maxMouseDistance && distanceToPlayer >= minHookDistance && RayHitPlayer(targetPoint) && canGrapple)
        {
            umbrella.hookTarget = targetPoint.transform.position;


            GetComponent<PlayerStateHandler>().Grapple();
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            canGrapple = false;

            var hook = Instantiate(hookPrefab, transform.position, Quaternion.identity);
            var hookCS = hook.GetComponent<HookScript>();

            hookCS.targetPos = targetPoint.transform.position;
            hookCS.player = gameObject;
            hookCS.ropeStart = ropeStart;

            hookCS.hookSpeed = hookSpeed;
            hookCS.playerSpeed = playerSpeed;
            hookCS.playerAcceleration = playerAcceleration;
            hookCS.playerSpeedOverTime = playerSpeedOverTime;

            if (targetPoint.transform.parent != null && targetPoint.transform.parent.CompareTag("Enemy"))
            {
                hookCS.target = targetPoint;
                targetPoint.GetComponentInParent<EnemyHandler>().currentMode = EnemyHandler.Mode.Idle;
                targetPoint.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }

            umbrella.soundHandler.PlaySound(umbrella.clips[0]);
        }
        else
            umbrella.Idle();
    }

    private void OnDrawGizmos()
    {
        if (showGrappleLeangth)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, hookMaxReach);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,Mathf.Sqrt(minHookDistance));
        }
    }
}
