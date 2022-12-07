using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float extraLeangth = 1f;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;

    public PhysicsMaterial2D groundMaterial;
    public PhysicsMaterial2D playerMaterial;

    private PlayerStateHandler stateManager;

    private float groundAngle;
    private Vector2 groundDirection;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        stateManager = GetComponent<PlayerStateHandler>();
    }

    private void Update()
    {
        if (stateManager.isGrounded != IsGrounded())
        {
            stateManager.isGrounded = IsGrounded();
            circleCollider.sharedMaterial = IsGrounded() ? groundMaterial : playerMaterial;
        }
    }

    private void FixedUpdate()
    {
        if (groundAngle < 145 && groundAngle != 0)
        {
            stateManager.slopeSlide();
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D rayHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraLeangth, groundLayer);

        groundAngle = Vector3.Angle(rayHit.normal, Vector2.down);

        //Visual only
        Color rayColor;
        if (rayHit.collider != null && groundAngle > 145)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraLeangth), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraLeangth), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + extraLeangth), Vector2.right * (boxCollider.bounds.extents.x * 2), rayColor);

        // Debug.Log(groundAngle);

        if (groundAngle > 145)
            return rayHit.collider != null;
        else
            return false;
    }



}
