using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    PlayerStateHandler stateHandler;
    private bool knockedBack = false;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        flipPlayerCS = GetComponent<FlipPlayer>();
        stateHandler = GetComponent<PlayerStateHandler>();
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

        if (!knockedBack)
        {
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
    }

    public void Knockback(float force, Vector2 hitPosition)
    {
        knockedBack = true;
        stateHandler.currentMoveState = PlayerStateHandler.MovementStates.Knockback;
        Vector2 direction = (Vector2)transform.position - hitPosition;
        direction.Normalize();
        GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        Invoke(nameof(Knockbackfalse), 0.1f);
    }

    private void Knockbackfalse() 
    {
        knockedBack = false;
        stateHandler.currentMoveState = PlayerStateHandler.MovementStates.Idle;
    }

    public Vector2 PublicMovementVector()
    {
        return currentVelocity;
    }
}
