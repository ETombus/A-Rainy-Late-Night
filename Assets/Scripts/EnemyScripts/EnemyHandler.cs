using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;

    [Header("Movement")]
    public EnemyMovement movement;

    [Header("Detection")]
    public EnemyDetect detection;
    public EnemyEdgeDetection edgeDetection;

    [Header("Attacking")]
    public EnemyShooting shooting;
    public EnemyMelee melee;

    [Header("GlobalVariables")]
    public bool isAttacking;

    public enum Mode { Patrol, Aggression, Search, Idle }
    public enum Type { Melee, Ranged }
    [Header("Enums")]
    public Mode currentMode;
    public Type thisType;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        movement = GetComponent<EnemyMovement>();

        if (thisType == Type.Ranged)
            shooting = GetComponent<EnemyShooting>();
        else if (thisType == Type.Melee)
            melee = GetComponent<EnemyMelee>();            

        detection = GetComponentInChildren<EnemyDetect>();
        edgeDetection = GetComponentInChildren<EnemyEdgeDetection>();
    }
}
