using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] public AnimationCurve playerSpeedOverTime;
    public GameObject player;
    public GameObject target;
    public Vector2 targetPos;
    public Transform ropeStart;
    LineRenderer rope;

    [Header("Values")]
    [HideInInspector] public float hookSpeed;
    [HideInInspector] public float playerSpeed;
    [HideInInspector] public float playerAcceleration;
    float percentage;
    float failsafe;


    private void Start()
    {
        rope = GetComponent<LineRenderer>();
        rope.positionCount = 2;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, hookSpeed * Time.deltaTime);
        Vector2 dir = transform.position - ropeStart.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        rope.SetPosition(0, ropeStart.position);
        rope.SetPosition(1, transform.position);

        if ((Vector2)transform.position == targetPos)
        {
            failsafe += Time.deltaTime;
            percentage += Time.deltaTime * playerAcceleration;
            percentage = Mathf.Clamp(percentage, 0, playerSpeedOverTime.length);

            var playerRB = player.GetComponent<Rigidbody2D>();
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRB.velocity = (dir * playerSpeedOverTime.Evaluate(percentage) * playerSpeed);

            if (Vector2.SqrMagnitude((Vector2)player.transform.position - targetPos) < 1f)
            {
                player.GetComponent<Rigidbody2D>().velocity += new Vector2(0, dir.normalized.y * Mathf.Abs(dir.normalized.x) * 10);
                ResetHook();
            }
            else if(failsafe >= 1f)
            {
                ResetHook();
            }
        }
    }

    public void ResetHook()
    {
        if (target != null)
            target.GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        
        player.GetComponentInChildren<UmbrellaStateHandler>().Idle();
        player.GetComponent<PlayerStateHandler>().DisableGrappleGravity();
        player.GetComponent<GrappleInput>().canGrapple = true;
        Destroy(gameObject);
    }
}
