using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint")]
    public int index = 1; //index 0 is the starting position for the player, the first checkpoint should be 1
    public bool reached = false;

    [Header("Settings")]
    [SerializeField] bool useStandardPos = true; //if true, respawnposition gets set to the checkpoints position
    public Vector2 respawnPos;

    CheckpointManager manager;

    [Header("Audio")]
    [SerializeField] AudioClip clip;
    AudioSource audSource;
    

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<CheckpointManager>();
        audSource = GetComponent<AudioSource>();

        if (useStandardPos)
            respawnPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !reached)
        {
            collision.gameObject.GetComponent<Healthbar>().AddHealth(100, true);

            reached = true;
            manager.UpdateCheckpoints(index);
            audSource.PlayOneShot(clip);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(respawnPos, 0.5f);
    }
}
