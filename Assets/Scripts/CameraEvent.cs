using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEvent : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] bool isInField;
    [SerializeField] bool bufferPeriod;
    [SerializeField] float buffertime = 0.3f;
    [SerializeField] float buffertimer;

    [Header("Camera Variables")]
    [SerializeField] Transform cameraPos;
    [SerializeField] CinemachineVirtualCamera vCam;
    float camSize;
    [SerializeField] float newCamSize = 7;
    [SerializeField] float transitionSpeed = 4;
    [SerializeField] float t;

    Transform cameraOrigin;

    [SerializeField] GameObject[] imgsToShow;

    // Start is called before the first frame update
    void Start()
    {
        cameraPos = this.gameObject.transform.GetChild(0);
        vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        camSize = vCam.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        if (isInField && vCam.m_Lens.OrthographicSize < newCamSize)
        {
            vCam.m_Lens.OrthographicSize = Mathf.Lerp(camSize, newCamSize, t);

            t += 0.5f * Time.deltaTime;
        }
        else if (!isInField && vCam.m_Lens.OrthographicSize > camSize)
        {
            vCam.m_Lens.OrthographicSize = Mathf.Lerp(newCamSize, camSize, t);

            t += 0.5f * Time.deltaTime;
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


        vCam.Follow = cameraOrigin;
        t = 0;

        if (imgsToShow.Length > 0)
        {
            for (int i = 0; i < imgsToShow.Length; i++)
            {
                imgsToShow[i].SetActive(false);
            }
        }
    }
}
