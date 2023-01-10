using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] EnemySpineController enemySpine;

    [Header("Player")]
    public GameObject player;
    public Healthbar playerHealth;
    public Transform playerTrans;

    [Header("Movement")]
    public EnemyMovement movement;
    public bool isMoving = false;
    public Transform objectThatNeedFlippin;

    [Header("Detection")]
    public EnemyDetect detection;
    public EnemyEdgeDetection edgeDetection;

    [Header("Attacking")]
    public EnemyShooting shooting;
    public EnemyMelee melee;

    [Header("GlobalVariables")]
    public bool isAttacking;
    public bool isOnLedge;

    [Header("Sound")]
    public AudioClip[] clips;
    AudioSource audSource;
    //0 = punch, 1 = shoot

    private void Update()
    {
        if (objectThatNeedFlippin != null)
            objectThatNeedFlippin.eulerAngles = new Vector3(0, 0, 0);
    }

    public enum Mode { Patrol, Aggression, Search, Idle, Dead, Working }
    public enum Type { Melee, Ranged }
    [Header("Enums")]
    public Mode currentMode;
    public Type thisType;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Healthbar>();
        playerTrans = player.GetComponent<Transform>();
        enemySpine = GetComponentInChildren<EnemySpineController>();

        movement = GetComponent<EnemyMovement>();
        if (movement == null) { Debug.LogError(gameObject.name + " is missing Movement!"); }

        audSource = GetComponent<AudioSource>();
        if (audSource == null) { Debug.LogError(gameObject.name + " is missing AudioSource!"); }

        if (thisType == Type.Ranged)
        {
            shooting = GetComponent<EnemyShooting>();
            if (shooting == null) { Debug.LogError(gameObject.name + " is marked ranged and should have the EnemyShooting script! Have you not added it or is EnemyType wrong?"); }
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
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
    }

    public void FlipRotation()
    {
        FlipRotation(movement.movePoints[movement.moveIndex].x - transform.position.x);
    }

    public void AlertEnemy()
    {
        currentMode = Mode.Aggression;
    }

    public void EnemyDead()
    {
        detection.markerRenderer.sprite = null;
        audSource.PlayOneShot(enemySpine.damageSound);
        currentMode = Mode.Dead;
    }

    public void PlaySound(EnemyHandler.Type typeOfEnemy)
    {
        audSource.pitch = Random.Range(0.8f, 1.2f);
        switch (typeOfEnemy)
        {
            case Type.Melee:
                audSource.PlayOneShot(clips[0]);
                break;
            case Type.Ranged:
                audSource.PlayOneShot(clips[1]);
                break;
            default:
                Debug.LogError("Cannot play sound effect, invalid EnemyType! Has this enemy been assigned an EnemyType?");
                break;
        }
    }
}
