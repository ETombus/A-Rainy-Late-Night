using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{


    [SerializeField]
    private GameObject player;

    public LayerMask detectableLayers;

    [SerializeField]
    private bool playerVisable = false;

    private bool playerInViewRange = false;

    private int colliderIndex;

    private void Update()
    {
        if (playerInViewRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 20, detectableLayers);
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);

            if (hit.collider.gameObject.layer == 3) // Terrain Layer
            {
                playerVisable = false;
            }
            else
                playerVisable = true;

            Debug.Log(hit.collider.gameObject);
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
