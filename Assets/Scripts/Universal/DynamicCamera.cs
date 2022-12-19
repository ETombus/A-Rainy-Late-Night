using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DynamicCamera : MonoBehaviour
{
    //Components
    PlayerStateHandler stateHandler;

    //Vectors
    Vector2 startPos;

    [Header("Variables")]
    [SerializeField] float cameraSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] float lookUpDist = 5;
    float xPos;
    float yPos;

    [Header("BufferTime")]
    [SerializeField] float bufferTime = 1f;
    [SerializeField] float bufferTimer;
    [SerializeField] float changeDirBuffer = 0.8f; //time it takes for the camera to switch direction when the player does so
    [SerializeField] float dirBufferTimer;

    float lastInput;


    private void Start()
    {
        startPos = transform.localPosition;
        xPos = startPos.x;
        yPos = startPos.y;

        stateHandler = GetComponentInParent<PlayerStateHandler>();

        lastInput = stateHandler.inputX;
    }

    // Update is called once per frame
    void Update()
    {
        CameraFollowHorizontal();

        CameraLookUp();
    }


    private void CameraFollowHorizontal()
    {

        if (stateHandler.inputX != 0 && lastInput == stateHandler.inputX)
        {
            bufferTimer += Time.deltaTime;
        }
        else
        {
            transform.localPosition = startPos;
            xPos = startPos.x;
            bufferTimer = 0;
        }

        lastInput = stateHandler.inputX;

        if (bufferTimer >= bufferTime)
        {
            if (stateHandler.inputX > 0)
            {
                if (xPos < startPos.x)
                {
                    dirBufferTimer += Time.deltaTime;

                    if (dirBufferTimer > changeDirBuffer)
                        xPos = startPos.x;
                }
                else { dirBufferTimer = 0; }

                xPos += cameraSpeed * Time.deltaTime;
                //transform.localPosition += moveVector;
            }
            else if (stateHandler.inputX < 0)
            {
                if (xPos > startPos.x)
                {
                    dirBufferTimer += Time.deltaTime;

                    if (dirBufferTimer > changeDirBuffer)
                        xPos = -startPos.x;
                }
                else { dirBufferTimer = 0; }

                xPos -= cameraSpeed * Time.deltaTime;

                //transform.localPosition -= moveVector;
            }

        }

        xPos = Mathf.Clamp(xPos, -maxDistance, maxDistance);
    }

    private void CameraLookUp()
    {
        var mousePos = Mouse.current.position.ReadValue();
        //Debug.Log(mousePos);

        if (mousePos.y > Camera.main.pixelHeight * 0.85f)
        {
            yPos = lookUpDist;
        }
        else if (mousePos.y < Camera.main.pixelHeight * 0.5f)
        {
            yPos = 0;
        }

        transform.localPosition = new(xPos, yPos);
    }
}
