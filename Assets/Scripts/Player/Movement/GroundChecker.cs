using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Windows;

public class GroundChecker : MonoBehaviour
{
    [Header("Ground Check")]
    public LayerMask groundLayer;
    private bool hitGround = false;


    public float extraLeangth = 1f;


    private BoxCollider2D boxCollider;


    [Header("Material")]
    public PhysicsMaterial2D fullFrictionMaterial;
    public PhysicsMaterial2D noFrictionMaterial;

    private PlayerStateHandler stateManager;


    [Header("Slope Check")]
    private CircleCollider2D circleCollider;

    public float slopeCheckDistance = 0.3f;
    public float maxSlopeAngle = 145f;


    private bool isOnSlope = false;
    private bool canWalkOnSlope = false;
    private float slopeSideAngle;
    private float slopeDownAngle;
    private float lastSlopeAngle;



    private Vector2 slopeNormalPerp;

    private Rigidbody2D rbody;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        stateManager = GetComponent<PlayerStateHandler>();
        rbody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
<<<<<<< Updated upstream
        if (stateManager.isGrounded != IsGrounded())
        {
            stateManager.isGrounded = IsGrounded();
            circleCollider.sharedMaterial = IsGrounded() ? groundMaterial : playerMaterial;
        }
=======


        if (stateManager.isGrounded != hitGround)
            stateManager.isGrounded = hitGround;

        if (stateManager.onSlope != isOnSlope)
            stateManager.onSlope = isOnSlope;

        if (stateManager.walkableSlope != canWalkOnSlope)
            stateManager.walkableSlope = canWalkOnSlope;

>>>>>>> Stashed changes
    }


    private void FixedUpdate()
    {
        IsGrounded();
        SlopeCheck();

        stateManager.slopeDirection = slopeNormalPerp;
    }



    private void IsGrounded()
    {
        RaycastHit2D rayHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraLeangth, groundLayer);


        //Visual only
        Color rayColor;
        if (rayHit.collider != null)
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

<<<<<<< Updated upstream
        // Debug.Log(groundAngle);
=======
        if (rayHit.collider != null)
            hitGround = true;
        else
            hitGround = false;

    }

    void SlopeCheck()
    {
        Vector2 checkPos = transform.position + (Vector3)(new Vector2(0, circleCollider.offset.y - circleCollider.radius));


        slopeCheckHorizontal(checkPos);
        slopeCheckVertical(checkPos);
    }

    void slopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, groundLayer);


        Vector2 endPos1 = slopeHitFront ? slopeHitFront.point : checkPos + new Vector2(slopeCheckDistance, 0);
        Vector2 endPos2 = slopeHitBack ? slopeHitBack.point : checkPos - new Vector2(slopeCheckDistance, 0);
        Debug.DrawLine(checkPos, endPos1, Color.blue);
        Debug.DrawLine(checkPos, endPos2, Color.blue);

>>>>>>> Stashed changes


        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    void slopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        Vector2 endPos = hit ? hit.point : checkPos - new Vector2(0, slopeCheckDistance);
        Debug.DrawLine(checkPos, endPos, Color.blue);


        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            if ((hit.point - checkPos).magnitude < 0.01f && slopeSideAngle > maxSlopeAngle)
            {
                isOnSlope = false;
                
            }

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);

            Debug.Log(slopeDownAngle + " , " + slopeSideAngle);

            if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
            {
                canWalkOnSlope = false;
            }
            else
            {
                canWalkOnSlope = true;
            }

            if (isOnSlope && canWalkOnSlope && stateManager.inputX == 0.0f)
            {
                rbody.sharedMaterial = fullFrictionMaterial;
            }
            else
            {
                rbody.sharedMaterial = noFrictionMaterial;
            }
        }
    }


}
