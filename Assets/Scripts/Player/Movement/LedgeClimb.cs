using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeClimb : MonoBehaviour
{
    private PlayerStateHandler stateHandeler;

    public Transform wallCheck;
    public Transform ledgeCheck;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;



    public float wallCheckDistance = 5;
    public LayerMask whatIsTerrain;

    private bool isTuchingLedge = false;
    private bool isTuchingWall = false;

    private bool canClimbLedge = false;

    [SerializeField]private bool ledgeDetected = false;

    private void Start()
    {
        stateHandeler = GetComponentInParent<PlayerStateHandler>();
    }



    void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge=true;
        }
    }

    private void Update()
    {        
        CheckDetection();
    }

    void CheckDetection()
    {
        Color raycolor;
        isTuchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsTerrain);


        isTuchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsTerrain);

        raycolor = isTuchingWall ? Color.red : Color.green;
        Debug.DrawRay(wallCheck.position, transform.right * wallCheckDistance, raycolor);
        raycolor = isTuchingLedge ? Color.red : Color.green;
        Debug.DrawRay(ledgeCheck.position, transform.right * wallCheckDistance, raycolor);




        if (isTuchingWall && !isTuchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
        }
    }


}
