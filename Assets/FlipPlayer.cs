using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class FlipPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Transform umbrellaTrans;
    [SerializeField] Transform aimTransform;
    [SerializeField] Transform cameraTarget;
    [SerializeField] float cameraTargetXPos;
    SpriteRenderer umbrellaSR;
    Vector2 umbrellaPos;
    Vector2 aimPos;
    public static bool flippedX;
    public static bool overrideFlip;


    private void Awake()
    {
        playerSprite = GetComponentInChildren<SpriteRenderer>(false);

        flippedX = false;
        umbrellaSR = umbrellaTrans.gameObject.GetComponent<SpriteRenderer>();
    }

    public void FlipPlayerX()
    {
        playerSprite.flipX = !playerSprite.flipX;
        umbrellaSR.flipX = !umbrellaSR.flipX;

        cameraTargetXPos = cameraTarget.localPosition.x;
        umbrellaPos = umbrellaTrans.localPosition;
        aimPos = aimTransform.localPosition;

        cameraTargetXPos *= -1;
        umbrellaPos.x *= -1;
        aimPos.x *= -1;

        cameraTarget.localPosition = new(cameraTargetXPos, 0);
        umbrellaTrans.localPosition = new(umbrellaPos.x, umbrellaPos.y);
        aimTransform.localPosition = new(aimPos.x, aimPos.y);
    }
}
