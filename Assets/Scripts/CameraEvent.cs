using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEvent : MonoBehaviour
{
    [SerializeField] Transform cameraPos;
    [SerializeField] CinemachineVirtualCamera vCam;

    Transform cameraOrigin;

    // Start is called before the first frame update
    void Start()
    {
        cameraPos = this.gameObject.transform.GetChild(0);
        vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            vCam.Follow = cameraPos;
            cameraOrigin = collision.transform.GetChild(0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        vCam.Follow = cameraOrigin;
    }
}
