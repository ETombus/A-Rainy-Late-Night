using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    [Header("LayerMask")]
    public LayerMask detectableLayers;

    [Header("Detection and Visibility")]
    public float detectTime = 1;
    [SerializeField] private float timer = 0;
    [SerializeField] private bool playerVisable = false;
    [SerializeField] private GameObject player;
    public Vector2 lastSeenPlayerLocation;

    EnemyHandler handler;

    private bool playerInViewRange = false;

    private int colliderIndex;

    [SerializeField] Sprite[] indicators; // 0 = ?, 1 = !
    [SerializeField] SpriteRenderer indicatorRenderer;

    private void Start()
    {
        indicatorRenderer.gameObject.SetActive(false);
        handler = GetComponentInParent<EnemyHandler>();
    }

    private void Update()
    {
        if (playerInViewRange)
        {
            CheckIfBlockedByTerrain();
        }

        if (playerVisable) //only happens of CheckIfBlockedByTerrain returns false
        {
            //set indicator above enemies head
            indicatorRenderer.gameObject.SetActive(true);
            indicatorRenderer.sprite = indicators[0];

            handler.currentMode = EnemyHandler.Mode.Aggression; //stops the enemy from moving

            timer += Time.deltaTime; //allow the player some time to react, as long as timer is lower than detectTime

            //aggression mode
            if (timer >= detectTime)
            {
                indicatorRenderer.sprite = indicators[1];

                handler.shooting.isShooting = true;
            }
        } 

        //when the player leaves the viewarea, enter Search Mode
        //this is based on how long the player is within view
        if (!playerVisable && timer > 0)
        {
            handler.shooting.isShooting = false;

            timer -= Time.deltaTime;
            //if (timer < 0)
            //    timer = 0;

            // Change to Alert mode then possibly search mode
        }
        else if (!playerVisable && timer <= 0) //the enemy has lost sight and interest
        {
            indicatorRenderer.gameObject.SetActive(false);
            handler.currentMode = EnemyHandler.Mode.Patrol;
        }
    }

    void CheckIfBlockedByTerrain()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, detectableLayers);
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);

        if (hit.collider != null) // Terrain Layer
        {
            playerVisable = false;
        }
        else
        {
            playerVisable = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInViewRange = true;
            player = collision.gameObject;
            colliderIndex++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            lastSeenPlayerLocation = player.transform.position;
            Debug.Log(lastSeenPlayerLocation);

            colliderIndex--;
            if (colliderIndex == 0)
            {
                playerInViewRange = false;
                playerVisable = false;
                player = null;
            }
        }
    }
}
