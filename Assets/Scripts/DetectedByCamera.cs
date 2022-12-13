using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectedByCamera : MonoBehaviour
{
    Animator animator;

    bool lookingAtIt = false;

    [Range(0f, 185f)]
    public float distanceFromPlayer = 100;

    public string PopupText;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (IsVisibleToCamera() && !lookingAtIt)
        {
            ActivateMarker();
        }
        else if (!IsVisibleToCamera() && lookingAtIt)
        {
            DisableMarker();
        }
    }

    public bool IsVisibleToCamera()
    {
        //Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        //return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;

        //Whole Screen is ca 185 units
        return (transform.position - ScannerController.Instance.Player.transform.position).sqrMagnitude < distanceFromPlayer;
    }

    void ActivateMarker()
    {
        lookingAtIt = true;
        animator.SetTrigger("ResetAnim");
        if (PopupText != null || PopupText == "")
        {
            ScannerController.Instance.WriteText(PopupText, 0.1f);
            Debug.Log("Write "+PopupText );
        }

    }

    void DisableMarker()
    {
        lookingAtIt = false;
        animator.SetTrigger("RemoveMarker");
    }
}
