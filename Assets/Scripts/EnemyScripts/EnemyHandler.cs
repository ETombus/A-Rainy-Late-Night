using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Healthbar playerHealth;
    public Transform playerTrans;

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
    public bool isOnLedge;

    public enum Mode { Patrol, Aggression, Search, Idle }
    public enum Type { Melee, Ranged }
    [Header("Enums")]
    public Mode currentMode;
    public Type thisType;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Healthbar>();
        playerTrans = player.GetComponent<Transform>();

        movement = GetComponent<EnemyMovement>();

        if (thisType == Type.Ranged)
        {
            shooting = GetComponent<EnemyShooting>();
            if (shooting == null) { Debug.LogError(gameObject.name + " is marked ranged and should Have the EnemyShooting script! Have you not added it or is EnemyType wrong?"); }
        }
        else if (thisType == Type.Melee)
        {
            melee = GetComponent<EnemyMelee>();
            if (melee == null) { Debug.LogError(gameObject.name + "is marked as Melee and should have the EnemyMelee Script! Have you not added it or is EnemyType wrong?"); }
        }

        detection = GetComponentInChildren<EnemyDetect>();
        edgeDetection = GetComponentInChildren<EnemyEdgeDetection>();
    }

    public void FlipRotation(float direction)
    {
        if (direction < 0)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        else
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    }

    public void FlipRotation()
    {
        FlipRotation(movement.movePoints[movement.moveIndex].x - transform.position.x);
    }

    public void AlertEnemy()
    {
        currentMode = Mode.Aggression;
    }
}
