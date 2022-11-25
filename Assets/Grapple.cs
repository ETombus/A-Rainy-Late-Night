using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] GameObject player;
    Rigidbody2D rb2D;
    
    bool stuck;
    float maxDistance = 10f;
    public float grappleSpeed;

    void Start()
    {
        Vector2 startPos = transform.position;
        rb2D = GetComponent<Rigidbody2D>();
    }

    public IEnumerator GrappleHookStick()
    {
        Vector2 startPos = transform.position;
        
        stuck=false;

        do
        {
            if(Vector2.Distance(startPos, transform.position) >= maxDistance)
            {
                rb2D.velocity = (player.transform.position-transform.position)*(grappleSpeed/2);
                break;
            }
            yield return new WaitForEndOfFrame();
        }while(!stuck);
    }

    void StickToSurface(Collider2D hit)
    {
        if(!hit.CompareTag("Player"))
        {
            stuck=true;
            rb2D.velocity = Vector3.zero;
        }
    }
}
