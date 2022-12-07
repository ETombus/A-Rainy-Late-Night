using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeClimb : MonoBehaviour
{
    private PlayerStateHandler stateHandeler;
    private Rigidbody2D rbody;

    public Transform wallCheck;
    public Transform ledgeCheck;
    public Transform endPosCkeck;

    private Vector2 endPos;
    public Vector2 endPosOffset;

    public float climbSpeed;

    public float groundCheckDistance = 1;
    public float wallCheckDistance = 1;

    private bool isTuchingLedge = false;
    private bool isTuchingWall = false;


    bool facingRight = true;
    private bool isClimbLedge = false;
    [SerializeField] private bool ledgeDetected = false;
    [SerializeField] private bool wallDetected = false;

    public LayerMask whatIsTerrain;

    private void Start()
    {
        stateHandeler = GetComponentInParent<PlayerStateHandler>();
        rbody = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckDetection();
        CheckLedgeClimb();

        if (isClimbLedge)
            ClimbLedge();

        Turn();
    }

    void ClimbLedge()
    {
        rbody.simulated = false;

        transform.parent.position = Vector2.MoveTowards(transform.parent.position, endPos + endPosOffset, climbSpeed * Time.deltaTime);

        if (((Vector2)transform.parent.position - (endPos + endPosOffset)).magnitude <= 0)
        {
            resetClimb();
        }
    }

    void resetClimb()
    {
        rbody.simulated = true;
        rbody.velocity = Vector2.zero;
        stateHandeler.UpdateAcceleration();

        isClimbLedge = false;
    }

    void CheckLedgeClimb()
    {
        if (ledgeDetected && !isClimbLedge)
        {
            endPos = findEndPos();
            isClimbLedge = true;
        }
    }

    private Vector2 findEndPos()
    {
        RaycastHit2D hit = Physics2D.Raycast(endPosCkeck.position, Vector2.down, groundCheckDistance, whatIsTerrain);

        return hit.point;
    }

    void CheckDetection()
    {
        Color raycolor;
        isTuchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsTerrain);


        isTuchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance + 0.5f, whatIsTerrain);

        raycolor = isTuchingWall ? Color.red : Color.green;
        Debug.DrawRay(wallCheck.position, transform.right * wallCheckDistance, raycolor);
        raycolor = isTuchingLedge ? Color.red : Color.green;
        Debug.DrawRay(ledgeCheck.position, transform.right * (wallCheckDistance + 0.5f), raycolor);


        Debug.DrawRay(endPosCkeck.position, Vector2.down * groundCheckDistance);

        if (isTuchingWall && !isTuchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            wallDetected = false;
        }
        else if(isTuchingWall && isTuchingLedge && !wallDetected)
        {
            ledgeDetected = false;
            wallDetected = true;
        }
        else if (!isTuchingLedge && !isTuchingWall)
        {
            wallDetected = false;
            ledgeDetected = false;
        }
    }
    void Turn()
    {
        if(stateHandeler.inputX > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            facingRight = true;
        }
        else if(stateHandeler.inputX < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            facingRight = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(endPos, 0.2f);
    }

}
