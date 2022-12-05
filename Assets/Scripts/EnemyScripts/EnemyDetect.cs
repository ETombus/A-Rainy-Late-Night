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

        if (playerVisable)
        {
            indicatorRenderer.gameObject.SetActive(true);
            indicatorRenderer.sprite = indicators[0];

            handler.movement.isPatroling = false;
            
            timer += Time.deltaTime;
            if (timer >= detectTime)
            {
                indicatorRenderer.sprite = indicators[1];

                handler.shooting.isShooting = true;
                //Insert Agression mode here
            }
        }

        if (!playerVisable && timer > 0)
        {
            handler.shooting.isShooting = false;


            timer -= Time.deltaTime;
            //if (timer < 0)
            //    timer = 0;

            // Change to Alert mode then possibly search mode
        }
        else if (!playerVisable && timer <= 0)
        {
            indicatorRenderer.gameObject.SetActive(false);
            handler.movement.isPatroling = true;
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
