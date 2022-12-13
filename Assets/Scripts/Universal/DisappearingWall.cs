using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingWall : MonoBehaviour
{
    SpriteRenderer sprite;

    [SerializeField] bool bufferPeriod;
    [SerializeField] float buffertime = 0.2f;
    [SerializeField] float buffertimer;

    private void Start() { sprite = GetComponent<SpriteRenderer>(); }

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
            sprite.enabled = false;
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
        sprite.enabled = true;
        bufferPeriod = false;
        buffertimer = 0;
    }
}
