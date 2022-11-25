using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEvent : MonoBehaviour
{
    [SerializeField] Transform cameraPos;
    [SerializeField] CinemachineVirtualCamera vCam;

    Transform cameraOrigin;

    [SerializeField] GameObject[] imgsToShow;

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

            if (imgsToShow.Length > 0)
            {
                for (int i = 0; i < imgsToShow.Length; i++)
                {
                    imgsToShow[i].SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            vCam.Follow = cameraOrigin;

            if (imgsToShow.Length > 0)
            {
                for (int i = 0; i < imgsToShow.Length; i++)
                {
                    imgsToShow[i].SetActive(false);
                }
            }
        }
    }
}
