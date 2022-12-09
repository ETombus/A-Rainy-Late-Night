using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleInput : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform ropeStart;
    [SerializeField] LayerMask rayIgnore;
    UmbrellaStateHandler umbrella;
    GameObject[] hookPoints;
    GameObject targetPoint;
    string hookPointTag = "HookPoint";

    [Header("Inputs")]
    PlayerInputs playerControls;
    InputAction grappleAction;
    Vector2 mousePos;
    Vector2 worldPos;

    [Header("Values")]
    [Range(0, 25)] [SerializeField] float hookMaxReach;
    [Range(0, 25)] [SerializeField] float maxMouseDistance;
    [SerializeField] float hookSpeed;
    [SerializeField] float playerSpeed;
    [SerializeField] float playerAcceleration;
    [SerializeField] AnimationCurve playerSpeedOverTime;
    float closestHookDistance;

    public bool canGrapple = true;

    private void Start() { umbrella = GetComponentInChildren<UmbrellaStateHandler>(); }

    private void Awake()
    {
        hookPoints = GameObject.FindGameObjectsWithTag(hookPointTag);
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

        foreach (GameObject point in hookPoints)
        {
            float distance = Vector2.SqrMagnitude((Vector2)point.transform.position - worldPos);

            if (distance <= closestHookDistance && RayHitPlayer(point))
            {
                targetPoint = point;
                closestHookDistance = distance;
                point.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else if (distance > maxMouseDistance || !RayHitPlayer(point))
            {
                closestHookDistance = maxMouseDistance;
                point.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private bool RayHitPlayer(GameObject target)
    {
        Vector2 pos = transform.position;
        Vector2 targetPos = target.transform.position;
        Collider2D rayTarget = Physics2D.Raycast(targetPos, pos - targetPos, hookMaxReach, rayIgnore).collider;

        if (rayTarget == GetComponent<Collider2D>())
        { return true; }
        else
        { return false; }
    }

    public void ShootGrapple()
    {
        float distance = Vector2.SqrMagnitude((Vector2)targetPoint.transform.position - worldPos);

        if (distance <= maxMouseDistance && RayHitPlayer(targetPoint) && canGrapple)
        {
            GetComponent<PlayerStateHandler>().Grapple();
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

           umbrella.soundHandler.PlaySound(umbrella.clips[1]);
        }
        else
        {
            GetComponentInChildren<UmbrellaStateHandler>().Idle();
        }
    }
}
