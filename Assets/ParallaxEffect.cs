using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxEffect : MonoBehaviour
{
    [Header("Offset Values")]
    [Range(0, 10)][SerializeField] float offsetBetweenLayers;
    [Range(0, 10)][SerializeField] float closestLayerOffset;
    [Range(0.001f, 0.1f)][SerializeField] float speed;

    [Header("Layers")]
    [SerializeField] Transform[] backgroundLayers;
    float[] startPos;
    float[] layerOffsets;
    float[] reversLayerOffsets;

    Camera mainCam;
    Vector2 cameraMovement;

    void Start()
    {
        startPos = new float[backgroundLayers.Length];
        layerOffsets = new float[backgroundLayers.Length];
        layerOffsets[0] = closestLayerOffset;
        mainCam = Camera.main;

        for (int i = 1; i < layerOffsets.Length; i++)
        {
            layerOffsets[i] = layerOffsets[i - 1] + offsetBetweenLayers;
        }
        for(int i = 0; i < backgroundLayers.Length; i++)
        {
            startPos[i] = backgroundLayers[i].position.x;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            cameraMovement = mainCam.transform.position;
            //new Vector2(cameraMovement.x * layerOffsets[i], cameraMovement.y * (layerOffsets[i]/2));
            Debug.Log("i = " + i + " - " + layerOffsets[i]);

            backgroundLayers[i].position =
                new(startPos[i] + (cameraMovement.x * layerOffsets[i] * speed), backgroundLayers[i].position.y, backgroundLayers[i].position.z);

            float cameraX = mainCam.transform.position.x;
            float cameraHalfWidth = mainCam.orthographicSize * mainCam.aspect;
            float width = backgroundLayers[i].GetComponent<SpriteRenderer>().bounds.size.x;

            if (cameraX - cameraHalfWidth > startPos[i] + width/1.75f)
            {
                Debug.Log("right");
                startPos[i] += width;
            }
            else if (cameraX - cameraHalfWidth < startPos[i] - width / 1.75f)
            {
                Debug.Log("left");
                startPos[i] -= width;
            }
        }
    }
}