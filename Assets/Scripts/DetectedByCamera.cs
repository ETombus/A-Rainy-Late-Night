using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedByCamera : MonoBehaviour
{
    [SerializeField] float timeToShowMarker = 2;
    [SerializeField] GameObject marker;

    bool hasBeenSeen = false;

    // Update is called once per frame
    void Update()
    {
        if (!hasBeenSeen && IsVisibleToCamera(transform))
        {
            ActivateMarker();
        }
    }

    public bool IsVisibleToCamera(Transform transform)
    {
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }

    void ActivateMarker()
    {
        marker = transform.GetChild(1).gameObject;
        if (timeToShowMarker > 0)
        {
            marker.SetActive(true);
            timeToShowMarker -= Time.deltaTime;
        }
        else
        {
            marker.SetActive(false);
            hasBeenSeen = true;
        }
    }
}
