using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Jumping player;

    public float groundcheckDistance;
    public float bufferCheckDistance = 0.1f;
    public LayerMask groundLayer;

    // Moved to State handeler

    //public float extraLeangth = 1;
    //private BoxCollider2D boxCollider;

    private void Start()
    {
        //boxCollider = GetComponentInParent<BoxCollider2D>();
        player = GetComponentInParent<Jumping>();
    }

    private void Update()
    {
        groundcheckDistance = (GetComponentInParent<CapsuleCollider2D>().size.y / 2) + bufferCheckDistance;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundcheckDistance, groundLayer);

        if (hit.collider != null) { player.isGrounded = true; }
        else { player.isGrounded = false; }


    }

    // Moved to State handeler

    //private bool IsGrounded()
    //{
    //    RaycastHit2D rayHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraLeangth, groundLayer);
    //    Color rayColor;
    //    if (rayHit.collider != null)
    //    {
    //        rayColor = Color.green;
    //    }
    //    else
    //    {
    //        rayColor = Color.red;
    //    }


    //    Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraLeangth), rayColor);
    //    Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraLeangth), rayColor);
    //    Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + extraLeangth), Vector2.right * (boxCollider.bounds.extents.x * 2), rayColor);


    //    return rayHit.collider != null;
    //}



}
