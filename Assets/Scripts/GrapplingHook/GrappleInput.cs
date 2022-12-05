using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleInput : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject hookPrefab;
    GameObject targetPoint;
    GameObject[] hookPoints;
    string hookPointTag = "HookPoint";

    [Header("Inputs")]
    PlayerInputs playerControls;
    InputAction grappleAction;
    bool canGrapple = true;
    Vector2 mousePos;
    Vector2 worldPos;

    [Header("Values")]
    [Range(0, 25)][SerializeField] float hookMaxReach;
    [Range(0, 25)][SerializeField] float maxMouseDistance;
    [Range(0, 25)][SerializeField] float hookShootSpeed;
    [Range(0, 25)][SerializeField] float playerTravelSpeed;
    float closestHookDistance;

    private void Awake()
    {
        playerControls = new PlayerInputs();
        hookPoints = GameObject.FindGameObjectsWithTag(hookPointTag);
        closestHookDistance = maxMouseDistance;
    }

    private void OnEnable()
    {
        grappleAction = playerControls.Player.Grapple;
        grappleAction.Enable();
        grappleAction.performed += ShootGrapple;
    }

    private void OnDisable()
    {
        grappleAction.Disable();
    }

    private void FixedUpdate()
    {
        mousePos = Mouse.current.position.ReadValue();
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        foreach (GameObject point in hookPoints)
        {
            float distance = Vector2.Distance(point.transform.position, worldPos);

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
        Collider2D rayTarget = Physics2D.Raycast(targetPos, pos - targetPos, hookMaxReach).collider;

        if (rayTarget == GetComponent<Collider2D>())
            return true;
        else
            return false;
    }

    private void ShootGrapple(InputAction.CallbackContext context)
    {
        float distance = Vector2.Distance(targetPoint.transform.position, worldPos);

        Debug.Log("shootInput");
        if (distance <= maxMouseDistance && RayHitPlayer(targetPoint) && canGrapple)
        {
            Debug.Log("shoot");
            canGrapple = false;

            var hook = Instantiate(hookPrefab);
            var hookCS = hook.GetComponent<HookScript>();
            hookCS.targetPos = targetPoint.transform.position;
            hookCS.hookSpeed = hookShootSpeed;
            hookCS.playerSpeed = playerTravelSpeed;
            hookCS.player = gameObject;
        }
    }
}
