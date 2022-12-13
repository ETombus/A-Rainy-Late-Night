using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEvent : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] static bool isInField = false;
    [SerializeField] bool bufferPeriod;
    [SerializeField] float buffertime = 0.3f;
    [SerializeField] float buffertimer;

    [Header("Camera Variables")]
    [SerializeField] Transform cameraPos;
    [SerializeField] CinemachineVirtualCamera vCam;
    float originalCamSize;
    static float newCamSize;
    [SerializeField] float thisNewCamSize = 7;
    [SerializeField] float transitionSpeed = 4;

    Transform cameraOrigin;

    [Header("Non-Required")]
    [SerializeField] GameObject[] imgsToShow;

    // Start is called before the first frame update
    void Start()
    {
        cameraPos = this.gameObject.transform.GetChild(0);
        vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        originalCamSize = vCam.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        if (isInField && vCam.m_Lens.OrthographicSize < newCamSize)
        {
            vCam.m_Lens.OrthographicSize += Time.deltaTime * transitionSpeed;
        }
        else if (!isInField && vCam.m_Lens.OrthographicSize > originalCamSize)
        {
            vCam.m_Lens.OrthographicSize -= Time.deltaTime * transitionSpeed;
        }

        if (bufferPeriod)
        {
            buffertimer += Time.deltaTime;
            if (buffertimer >= buffertime) { ReturnCamera(); }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            newCamSize = thisNewCamSize;

            bufferPeriod = false;
            buffertimer = 0;

            vCam.Follow = cameraPos;
            cameraOrigin = collision.transform.GetChild(0);
            isInField = true;

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
            bufferPeriod = true;
    }

    void ReturnCamera()
    {
        isInField = false;
        bufferPeriod = false;

        Debug.Log(cameraOrigin);
        vCam.Follow = cameraOrigin;

        //cameraOrigin = null;

        if (imgsToShow.Length > 0)
        {
            for (int i = 0; i < imgsToShow.Length; i++)
            {
                imgsToShow[i].SetActive(false);
            }
        }
    }
}
