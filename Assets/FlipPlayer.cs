using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class FlipPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform cameraTarget;
    [SerializeField] float cameraTargetXPos;

    public static bool flippedX;


    private void Start()
    {
        flippedX = false;
    }

    //public void MoveCameraInbetween(Vector2 secondPosition)
    //{
    //    cameraTarget.position = secondPosition + ((Vector2)transform.position - secondPosition)/1.5f;
    //}

    public void FlipPlayerX()
    {


    }
}
