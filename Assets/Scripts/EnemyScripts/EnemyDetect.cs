using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public LayerMask detectableLayers;

    public float detectTime = 1;

    [SerializeField]
    private float timer = 0;

    [SerializeField]
    private bool playerVisable = false;

    [SerializeField]
    private GameObject player;

    private bool playerInViewRange = false;

    private int colliderIndex;

    private void Update()
    {
        if (playerInViewRange)
        {
            CheckIfBlockedByTerrain();
        }

        if (playerVisable)
        {
            timer += Time.deltaTime;

            if (timer >= detectTime)
            {
                //Debug.Log("Fire at player!");
                //Insert Agression mode here
            }
        }

        if (!playerVisable && timer > 0)
        {
            Debug.Log("Alert");
            timer -= Time.deltaTime;
            if (timer < 0)
                timer = 0;

            // Change to Alert mode then possibly search mode
        }
    }

    void CheckIfBlockedByTerrain()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, detectableLayers);
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);

        if (hit.collider.gameObject.layer == 3) // Terrain Layer
        {
            playerVisable = false;
        }
        else
        {
            playerVisable = true;
        }

        //Debug.Log("Currently looking at: " + hit.collider.gameObject.tag);
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
