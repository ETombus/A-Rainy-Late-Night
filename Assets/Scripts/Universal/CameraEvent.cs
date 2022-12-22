using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEvent : MonoBehaviour
{
    private enum EventType { sizeAndPos, size, position }
    [SerializeField] private EventType thisEventType;

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
        vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        if (thisEventType == CameraEvent.EventType.size || thisEventType == CameraEvent.EventType.sizeAndPos)
            originalCamSize = vCam.m_Lens.OrthographicSize;

        if (thisEventType != CameraEvent.EventType.size)
        {
            cameraPos = this.gameObject.transform.GetChild(0);
        }
    }

    private void Update()
    {
        if (thisEventType == CameraEvent.EventType.size || thisEventType == CameraEvent.EventType.sizeAndPos)
        {
            if (isInField && vCam.m_Lens.OrthographicSize < newCamSize)
            {
                vCam.m_Lens.OrthographicSize += Time.deltaTime * transitionSpeed;
            }
            else if (!isInField && vCam.m_Lens.OrthographicSize > originalCamSize)
            {
                vCam.m_Lens.OrthographicSize -= Time.deltaTime * (transitionSpeed / 4);
            }
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

            isInField = true;

            switch (thisEventType)
            {
                case EventType.sizeAndPos:
                    newCamSize = thisNewCamSize;
                    vCam.Follow = cameraPos;
                    cameraOrigin = collision.transform.GetChild(0);
                    break;
                case EventType.size:
                    newCamSize = thisNewCamSize;
                    break;
                case EventType.position:
                    vCam.Follow = cameraPos;
                    cameraOrigin = collision.transform.GetChild(0);
                    break;
                default:
                    Debug.LogError("Invalid Camera Event Type on Camera Event " + gameObject.name);
                    break;
            }

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

        if (thisEventType == CameraEvent.EventType.position || thisEventType == CameraEvent.EventType.sizeAndPos)
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

    private void OnDrawGizmos()
    {
        if (cameraPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(cameraPos.position, new Vector3(newCamSize * 2, newCamSize * 2, 1));
        }
    }
}
