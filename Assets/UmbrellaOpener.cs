using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaOpener : MonoBehaviour
{

    [SerializeField] Sprite[] umbrellaSprites; //0 = open, 1 = closed
    [SerializeField] float distToCheck = 10f; //distance above the umbrella to check for roof
    [SerializeField] LayerMask layersToCheck; //what layers counts as roof
    public bool umbrellaOverrideBool; //to use: set overrideBool to true, then call for Open- or CloseUmbrella() dont forget to reset the bool to false
    //when resetting the bool, dont call for Open- / CloseUmbrella() !!

    //Private Components
    SpriteRenderer spriteRend;
    BoxCollider2D umbrellaCollider;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        umbrellaCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //search for ceiling above the player via raycast, if no ceiling is found it is presumed the player is being rained on
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, distToCheck, layersToCheck);

        if (hit.collider == null && !umbrellaOverrideBool) { OpenUmbrella(); }
        else if (!umbrellaOverrideBool) { CloseUmbrella(); }
    }

    public void OpenUmbrella()
    {
        spriteRend.sprite = umbrellaSprites[0];
        umbrellaCollider.enabled = true;
    }

    public void CloseUmbrella()
    {
        spriteRend.sprite = umbrellaSprites[1];
        umbrellaCollider.enabled = false;
    }
}
