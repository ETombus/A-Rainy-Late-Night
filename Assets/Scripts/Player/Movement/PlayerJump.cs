using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class PlayerJump : MonoBehaviour
{
    public float yJumpForce = 10;
    public float xJumpForce = 10;

    public float grapplexJumpForce = 8;

    private Rigidbody2D rbody;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    public void Jump(float horizontalInput)
    {

        //Debug.Log("IsJumping");

        rbody.velocity = new(rbody.velocity.x, 0);
        rbody.AddForce(new Vector3(xJumpForce * horizontalInput, yJumpForce), ForceMode2D.Impulse);
    }
}
