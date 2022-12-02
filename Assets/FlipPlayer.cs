using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class FlipPlayer : MonoBehaviour
{
    [Header("Components")]
    //[SerializeField] Transform umbrellaTrans;
    [SerializeField] Transform aimTransform;
    [SerializeField] Transform cameraTarget;
    [SerializeField] float cameraTargetXPos;
    //SpriteRenderer umbrellaSR;
    Vector2 umbrellaPos;
    Vector2 aimPos;
    public static bool flippedX;
    public static bool overrideFlip;


    private void Awake()
    {

        flippedX = false;
        //umbrellaSR = umbrellaTrans.gameObject.GetComponent<SpriteRenderer>();
    }

    public void MoveCameraInbetween(Vector2 secondPosition)
    {
        
        cameraTarget.position = secondPosition + ((Vector2)transform.position - secondPosition)/1.5f;
    }
    public void FlipPlayerX()
    {
        //umbrellaSR.flipX = !umbrellaSR.flipX;

        //umbrellaPos = umbrellaTrans.localPosition;
        aimPos = aimTransform.localPosition;

        cameraTargetXPos *= -1;
        umbrellaPos.x *= -1;

        //umbrellaTrans.localPosition = new(umbrellaPos.x, umbrellaPos.y);
    }
}
