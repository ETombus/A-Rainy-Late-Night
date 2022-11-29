using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaOpener : MonoBehaviour
{
    [SerializeField] Sprite[] umbrellaSprites; //0 = open, 1 = closed
    SpriteRenderer spriteRend;
    [SerializeField] float distToCheck = 10f;
    [SerializeField] LayerMask layersToCheck;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, distToCheck, layersToCheck);

        if (hit.collider == null) { spriteRend.sprite = umbrellaSprites[0]; }
        else { spriteRend.sprite = umbrellaSprites[1]; }
    }
}
