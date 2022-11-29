using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject player;
    [SerializeField] Transform hookParent;
    GrapplingHookShoot playerGrappleCS;
    LineRenderer rope;
    Rigidbody2D rb2D;
    
    [Header("Speeds")]
    public float grappleSpeed;
    [SerializeField] AnimationCurve grappleRetractSpeed;
    
    public static bool onPlayer = true;
    public static bool stuck = false;
    public static bool extended = false;
    public static float maxDistance = 25f;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerGrappleCS = player.GetComponent<GrapplingHookShoot>();
    }

    public IEnumerator GrappleHookStick()
    {
        Vector2 startPos = transform.position;
        
        onPlayer=false;

        do
        {
            if(Vector2.Distance(startPos, transform.position) >= maxDistance)
            {
                extended = true;
                Vector2 returnDirection = player.transform.position-transform.position;

                rb2D.velocity = returnDirection.normalized*(grappleSpeed*1.75f);

                yield return new WaitForSeconds(2f);

                if(!onPlayer)
                    SetParent();
            }
            yield return new WaitForEndOfFrame();
        }while(!stuck);
    }

    public void SetParent()
    {
        stuck=false;
        onPlayer=true;
        extended = false;
        gameObject.SetActive(false);
        playerGrappleCS.canGrapple=true;
        
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        transform.parent = hookParent;
        transform.localPosition = new Vector3(1f,0f,-1f);
        transform.localRotation = Quaternion.Euler(0,0,90);
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if(!hit.gameObject.CompareTag("Player"))
        {
            stuck=true;
            Debug.Log(hit.gameObject.name);

            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(MoveToHook());
        }
        else if(hit.gameObject.CompareTag("Player") && (extended || stuck))
        {
            SetParent();
        }
    }

    IEnumerator MoveToHook()
    {
        float frames=0;
        do
        {
            frames+=0.015f;
            frames = Mathf.Clamp(frames, 0f, 5f);
            player.GetComponent<Rigidbody2D>().velocity = 
                (transform.position-player.transform.position) * grappleRetractSpeed.Evaluate(frames)*Time.deltaTime;
            //player.transform.position = Vector2.Lerp(player.transform.position, transform.position, (grappleRetractSpeed.Evaluate(frames))*Time.deltaTime);
            //player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, grappleSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }while(Vector2.Distance(player.transform.position, transform.position) > 1);
        SetParent();
    }

}
