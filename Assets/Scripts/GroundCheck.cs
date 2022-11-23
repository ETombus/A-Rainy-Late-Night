using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Jumping player;

    public float groundcheckDistance;
    public float bufferCheckDistance = 0.1f;
    public LayerMask groundLayer;

    private void Start()
    {
        player = GetComponentInParent<Jumping>();
    }

    private void Update()
    {
        groundcheckDistance = (GetComponentInParent<CapsuleCollider2D>().size.y / 2) + bufferCheckDistance;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundcheckDistance, groundLayer);

        if (hit.collider != null) { player.isGrounded = true; }
        else { player.isGrounded = false; }
    }
}
