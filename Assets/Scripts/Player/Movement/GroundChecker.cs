using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float extraLeangth = 1f;
    private BoxCollider2D boxCollider;

    private PlayerStateHandler stateManager;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        stateManager = GetComponent<PlayerStateHandler>();
    }

    private void Update()
    {

        if (stateManager.isGrounded != IsGrounded())
            stateManager.isGrounded = IsGrounded();

    }

    private bool IsGrounded()
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


        return rayHit.collider != null;
    }
}
