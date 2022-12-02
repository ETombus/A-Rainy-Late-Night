using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Grapple : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject player;
    [SerializeField] Transform hookParent;
    GrapplingHookInputs playerGrappleCS;
    Rigidbody2D rb2D;

    [Header("Speeds")]
    [SerializeField] AnimationCurve grappleRetractSpeedOverTime;
    [Range(50, 100)][SerializeField] float grappleRetractSpeed;
    [Range(0, 5)][SerializeField] float grappleDelay = 1;
    [HideInInspector] public float grappleSpeed;
    public float returnMaxLifetime = 0.15f;

    public static bool stuck = false;
    public static bool onPlayer = true;
    public static bool extended = false;
    public static float maxDistance = 15f;
    public static float directionX;


    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerGrappleCS = player.GetComponent<GrapplingHookInputs>();
    }

    private void FixedUpdate()
    {
        if(extended)
        {
            Vector2 returnDirection = player.transform.position - transform.position;

            rb2D.velocity = returnDirection * (10);
            if ((player.transform.position - transform.position).magnitude < 3f)
            {
                SetParent();
            }
        }
    }

    public IEnumerator GrappleHookStick()
    {
        Vector2 startPos = transform.position;

        onPlayer = false;

        do
        {
            if (Vector2.Distance(startPos, transform.position) >= maxDistance)
            {
                extended = true;

                yield return new WaitForSeconds(returnMaxLifetime);

                if (!onPlayer)
                    SetParent();
            }
            yield return new WaitForEndOfFrame();
        } while (!stuck);
    }

    public void SetParent()
    {
        stuck = false;
        onPlayer = true;
        extended = false;
        gameObject.SetActive(false);
        
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        transform.parent = hookParent;
        transform.localPosition = new Vector3(1f, 0f, -1f);
        transform.localRotation = Quaternion.Euler(0, 0, 90);
        Invoke("Cooldown", grappleDelay);
    }

    void Cooldown()
    {
        playerGrappleCS.canGrapple = true;
        CancelInvoke();
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (!hit.gameObject.CompareTag("Player") && !extended && !hit.isTrigger)
        {
            stuck = true;
            Debug.Log(hit.gameObject.name);

            Vector2 hitPoint = hit.ClosestPoint(transform.position);
            float angle = Mathf.Atan2(hitPoint.y, hitPoint.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0,0,angle);
            
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(MoveToHook());
        }
        else if (extended || stuck)
        {
            SetParent();
        }
    }

    IEnumerator MoveToHook()
    {
        float frames = 0;
        do
        {
            frames += 0.015f;
            frames = Mathf.Clamp(frames, 0f, 5f);
            Vector2 hookDirection = (transform.position - player.transform.position).normalized;
            directionX = hookDirection.x > 0 ? 1 : -1;
            player.GetComponent<Rigidbody2D>().AddForce
                (grappleRetractSpeed * grappleRetractSpeedOverTime.Evaluate(frames) * Time.deltaTime * hookDirection);
            //player.transform.position = Vector2.Lerp(player.transform.position, transform.position, (grappleRetractSpeed.Evaluate(frames))*Time.deltaTime);
            //player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, grappleSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        } while (Vector2.Distance(player.transform.position, transform.position) > 3f);
        SetParent();
    }
}