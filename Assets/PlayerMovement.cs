using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jump;
    float inputX;
    Rigidbody2D rbody;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rbody.velocity = new(inputX * speed * Time.deltaTime, rbody.velocity.y);
    }

    public void Walking(InputAction.CallbackContext context)
    {
        Debug.Log("moving");
        inputX = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        rbody.velocity = new Vector2(rbody.velocity.x, 0);
        rbody.AddForce(Vector2.up * jump * Time.deltaTime * 1000, ForceMode2D.Impulse);
    }
}
