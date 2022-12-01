using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    GameObject[] backgroundLayers;
    void Start()
    {
        backgroundLayers = GetComponentsInChildren<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
