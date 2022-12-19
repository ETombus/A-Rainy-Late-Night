using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Walking : MonoBehaviour
{
    [Header("Ground Movement")]
    [SerializeField] float groundAcceleration = 40;
    [SerializeField] float groundDecceleration = 40f;
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
    public void Movement(float horizontalInput, bool onGround, bool onSlope, bool validSlope,Vector2 slopeDir)
    {

        if (onGround)
        {
            acceleration = groundAcceleration;
            decceleration = groundDecceleration;
            turnSpeed = groundTurnSpeed;
        }
        else
        {
            acceleration = airAcceleration;
            decceleration = airDecceleration;
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

        if (!onSlope)
        {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, horizontalInput * maxMoveSpeed, speedChange);
            currentVelocity.y = rbody.velocity.y;

            rbody.velocity = currentVelocity;
        }
        else if (onSlope && validSlope)
        {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, -horizontalInput * maxMoveSpeed * slopeDir.x, speedChange);
            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, -horizontalInput * maxMoveSpeed * slopeDir.y, speedChange);

            rbody.velocity = currentVelocity;
        }
        else
            UpdateCurrentVelocity();

    }

    public Vector2 PublicMovementVector()
    {
        return currentVelocity;
    }
}
