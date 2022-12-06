using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DynamicCamera : MonoBehaviour
{
    //Components
    Transform cameraTarget;
    Walking playerWalk;

    //Vectors
    Vector2 startPos;
    Vector3 moveVector;

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

    private void Start()
    {
        startPos = transform.localPosition;
        xPos = startPos.x;
        yPos = startPos.y;

        moveVector = new(cameraSpeed * Time.deltaTime, 0);

        playerWalk = GetComponentInParent<Walking>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraFollowHorizontal();

        CameraLookUp();
    }


    private void CameraFollowHorizontal()
    {
        if (playerWalk.PublicMovementVector().x != 0)
        {
            bufferTimer += Time.deltaTime;
        }

        if (bufferTimer <= bufferTime) { return; }

        if (playerWalk.PublicMovementVector().x > 0)
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
        else if (playerWalk.PublicMovementVector().x < 0)
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
        else if (playerWalk.PublicMovementVector().x == 0)
        {
            transform.localPosition = startPos;
            xPos = startPos.x;
            bufferTimer = 0;
        }

        xPos = Mathf.Clamp(xPos, -maxDistance, maxDistance);
    }

    private void CameraLookUp()
    {
        var mousePos = Mouse.current.position.ReadValue();
        //Debug.Log(mousePos);

        if (mousePos.y > Camera.main.pixelHeight * 0.85f)
        {
            Debug.Log("Looking Up");
            yPos = lookUpDist;
        }
        else if (mousePos.y < Camera.main.pixelHeight * 0.5f)
        {
            yPos = 0;
        }

        transform.localPosition = new(xPos, yPos);
    }
}
