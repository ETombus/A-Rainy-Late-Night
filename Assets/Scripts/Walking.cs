using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Walking : MonoBehaviour
{
    [Header("Ground Movement")]
    [SerializeField] float groundAcceleration = 40;
    [SerializeField] float groundAecceleration = 40f;
    [SerializeField] float groundTurnSpeed = 80;

    [Header("Air Movement")]
    [SerializeField] float airAcceleration = 40;
    [SerializeField] float airDecceleration = 40f;
    [SerializeField] float airTurnSpeed = 80;

    private float acceleration;
    private float decceleration;
    private float turnSpeed;

    [SerializeField] float maxMoveSpeed;

    private Vector2 currentVelocity;
    private float speedChange;

    [Header("Components")]
    private Rigidbody2D rbody;
    FlipPlayer flipPlayerCS;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        flipPlayerCS = GetComponent<FlipPlayer>();
    }

    public void UpdateCurrentVelocity()
    {
        currentVelocity = rbody.velocity;
    }

    public void Movement(float horizontalInput, bool onGround)
    {

        if (onGround)
        {
            acceleration = groundAcceleration;
            decceleration = groundAcceleration;
            turnSpeed = groundTurnSpeed;
        }
        else
        {
            acceleration = airAcceleration;
            decceleration = airAcceleration;
            turnSpeed = airTurnSpeed;
        }

        if (horizontalInput != 0)
        {
            if (Mathf.Sign(horizontalInput) != Mathf.Sign(currentVelocity.x))
            {
                speedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                speedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            speedChange = decceleration * Time.deltaTime;
        }

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, horizontalInput * maxMoveSpeed, speedChange);
        currentVelocity.y = rbody.velocity.y;
        rbody.velocity = currentVelocity;
    }

}
