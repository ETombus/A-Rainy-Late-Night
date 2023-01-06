using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEvent : MonoBehaviour
{
    private enum EventType { sizeAndPos, size, position }
    [SerializeField] private EventType thisEventType;

    private enum FollowType { DontFollow, FollowDown, FollowRight, FollowUp }
    [SerializeField] private FollowType thisFollowType;
    Vector2 startPos;

    private enum SizeType { Bigger, Smaller }
    [SerializeField] private SizeType thisSizeType;

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
    GameObject player;

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

        if (thisFollowType != FollowType.DontFollow)
        {
            startPos = cameraPos.position;
        }
    }

    private void Update()
    {
        if (thisEventType == CameraEvent.EventType.size || thisEventType == CameraEvent.EventType.sizeAndPos)
        {
            if (thisNewCamSize > originalCamSize && thisSizeType == SizeType.Bigger)
            {
                if (isInField && vCam.m_Lens.OrthographicSize < newCamSize)
                {
                    vCam.m_Lens.OrthographicSize += Time.deltaTime * transitionSpeed;
                }
                else if (isInField && vCam.m_Lens.OrthographicSize >= newCamSize) { vCam.m_Lens.OrthographicSize = newCamSize; }
                else if (!isInField && vCam.m_Lens.OrthographicSize > originalCamSize)
                {
                    vCam.m_Lens.OrthographicSize -= Time.deltaTime * (transitionSpeed / 4);
                }
            }
            else if (thisNewCamSize < originalCamSize && thisSizeType == SizeType.Smaller)
            {
                if (isInField && vCam.m_Lens.OrthographicSize > newCamSize)
                {
                    vCam.m_Lens.OrthographicSize -= Time.deltaTime * transitionSpeed;
                }
                else if (isInField && vCam.m_Lens.OrthographicSize <= newCamSize) { vCam.m_Lens.OrthographicSize = newCamSize; }
                else if (!isInField && vCam.m_Lens.OrthographicSize < originalCamSize)
                {
                    vCam.m_Lens.OrthographicSize += Time.deltaTime * (transitionSpeed / 4);
                }
            }
            else if (thisNewCamSize > originalCamSize && thisSizeType == SizeType.Smaller && thisEventType != EventType.position)
            { Debug.LogError(gameObject.name + " is marked as Smaller but has a larger newCamSize, is it incorrectly marked or is camsize incorrect?"); }
            else if (thisNewCamSize < originalCamSize && thisSizeType == SizeType.Bigger && thisEventType != EventType.position)
            { Debug.LogError(gameObject.name + " is marked as Bigger but has a smaller newCamSize, is it incorrectly marked or is camsize incorrect?"); }
        }

        if (thisFollowType == FollowType.FollowDown && player != null)
        {
            if (player.transform.position.y <= startPos.y)
            {
                cameraPos.transform.position = new(cameraPos.transform.position.x, player.transform.position.y);
            }
        }
        else if (thisFollowType == FollowType.FollowRight && player != null)
        {
            if (player.transform.position.x >= startPos.x)
            {
                cameraPos.transform.position = new(player.transform.position.x, cameraPos.transform.position.y);
            }
        }
        else if (thisFollowType == FollowType.FollowUp && player != null)
        {
            if (player.transform.position.y >= startPos.y)
            {
                cameraPos.transform.position = new(cameraPos.transform.position.x, player.transform.position.y);
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
            player = collision.gameObject;

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
            Gizmos.DrawWireCube(cameraPos.position, new Vector3(thisNewCamSize * 3.6f, thisNewCamSize * 2, 1));
        }
    }
}
