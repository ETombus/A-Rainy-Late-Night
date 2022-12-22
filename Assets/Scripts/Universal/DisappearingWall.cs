using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingWall : MonoBehaviour
{
    SpriteRenderer mainSprite;
    SpriteRenderer[] childSprites;

    [SerializeField] bool removeChildren = false;

    [SerializeField] bool bufferPeriod;
    [SerializeField] float buffertime = 0.2f;
    [SerializeField] float buffertimer;

    private void Start()
    {
        mainSprite = GetComponent<SpriteRenderer>();

        if (removeChildren)
            childSprites = GetComponentsInChildren<SpriteRenderer>();

    }

    private void Update()
    {
        if (bufferPeriod)
        {
            buffertimer += Time.deltaTime;
            if (buffertimer >= buffertime) { HideWall(); }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mainSprite.enabled = false;

            if (removeChildren)
                for (int i = 0; i < childSprites.Length; i++)
                {
                    childSprites[i].enabled = false;
                }

            bufferPeriod = false;
            buffertimer = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            bufferPeriod = true;
    }

    void HideWall()
    {
        mainSprite.enabled = true;

        if (removeChildren)
            for (int i = 0; i < childSprites.Length; i++)
            {
                childSprites[i].enabled = true;
            }

        bufferPeriod = false;
        buffertimer = 0;
    }
}
