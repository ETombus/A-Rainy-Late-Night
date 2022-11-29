using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class FlipPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Transform cameraTarget;
    [SerializeField] float cameraTargetXPos;
    [SerializeField] Transform umbrellaTrans;
    SpriteRenderer umbrellaSR;
    Vector2 umbrellaPos;
    public static bool flippedX;

    private void Awake()
    {
        playerSprite = GetComponentInChildren<SpriteRenderer>(false);

        cameraTargetXPos = cameraTarget.localPosition.x;
        umbrellaPos = umbrellaTrans.localPosition;
        umbrellaSR = umbrellaTrans.gameObject.GetComponent<SpriteRenderer>();
    }

    public void FlipPlayerX()
    {
        playerSprite.flipX = !playerSprite.flipX;
        umbrellaSR.flipX = !umbrellaSR.flipX;

        cameraTargetXPos *= -1;
        umbrellaPos.x *= -1;

        cameraTarget.localPosition = new(cameraTargetXPos, 0);
        umbrellaTrans.localPosition = new(umbrellaPos.x, umbrellaPos.y);
    }
}
