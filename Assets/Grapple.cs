using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject slice;
    GrapplingHookShoot playerGrappleCS;
    Rigidbody2D rb2D;
    
    bool stuck;
    bool onPlayer;
    bool returnToPlayer=false;
    float maxDistance = 10f;
    public float grappleSpeed;

    void Start()
    {
        Vector2 startPos = transform.position;
        rb2D = GetComponent<Rigidbody2D>();
        playerGrappleCS = player.GetComponent<GrapplingHookShoot>();
    }

    public IEnumerator GrappleHookStick()
    {
        Vector2 startPos = transform.position;
        
        stuck=false;
        onPlayer=false;
        returnToPlayer=false;

        do
        {
            if(Vector2.Distance(startPos, transform.position) >= maxDistance)
            {
                rb2D.velocity = (player.transform.position-transform.position)*(grappleSpeed/2);
                returnToPlayer=true;

                yield return new WaitForSeconds(2f);

                if(!onPlayer)
                    SetParent();
            }
            yield return new WaitForEndOfFrame();
        }while(!stuck);
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if(!hit.gameObject.CompareTag("Player"))
        {
            stuck=true;
            rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else if(hit.gameObject.CompareTag("Player") && returnToPlayer)
        {
            SetParent();
        }
    }

    void SetParent()
    {
        onPlayer=true;
        playerGrappleCS.canGrapple=true;
        gameObject.SetActive(false);
        transform.parent = slice.transform;
        transform.localPosition = new Vector3(1f,0f,-1f);
    }
}
