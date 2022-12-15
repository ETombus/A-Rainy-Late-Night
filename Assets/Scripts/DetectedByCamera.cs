using Spine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DetectedByCamera : MonoBehaviour
{
    Animator animator;

    bool lookingAtIt = false;

    [Range(0f, 185f)]
    public float maxDistanceFromPlayer = 100;

    [Range(0f, 185f)]
    public float minDistanceFromPlayer = 0;

    [Header("Hook Variables")]
    public bool isAHookPoint = false;
    public bool isSelected = false;

    [Header("Text Varibles")]
    public bool ShouldHaveText = false;
    public string textToWrite;

    public float timePerCharacter = 0.2f;
    public float timePerRemoval = 0.05f;

    private TextMeshProUGUI uiText;
    public GameObject canvas;

    int characterIndex;
    float timer;

    private bool writingText = false;
    private bool erasingText = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (ShouldHaveText)
        {
            if (canvas != null)
            {

                canvas.SetActive(true);
                uiText = GetComponentInChildren<TextMeshProUGUI>();
                uiText.text = "";
            }
            else
                Debug.LogError(gameObject.name + " is missing a canvas for text writing, disable ShouldHaveText to ignore problem");
        }
        else
        {
            if (canvas != null)
            {
                canvas.SetActive(false);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (CloseEnoughToPlayer(true) && !lookingAtIt)
        {
            ActivateMarker();
        }
        else if (!CloseEnoughToPlayer(true) && lookingAtIt)
        {
            DisableMarker();
        }

        if (isAHookPoint)
        {
            if (isSelected && CloseEnoughToPlayer(false))
                animator.SetBool("Selected", true);
            else
                animator.SetBool("Selected", false);
        }


        if (writingText)
            WriteText();
        else if (erasingText)
            EraseText();
    }

    public bool CloseEnoughToPlayer(bool returnMax)
    {
        //Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        //return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;

        float markerToPlayer = (transform.position - ScannerController.Instance.Player.transform.position).sqrMagnitude;

        //Whole Screen is ca 185 units

        if (returnMax)
            return markerToPlayer < maxDistanceFromPlayer;
        else
            return markerToPlayer > minDistanceFromPlayer;
    }

    void ActivateMarker()
    {
        lookingAtIt = true;
        animator.SetBool("MarkerVisable", true);

        if (ShouldHaveText)
        {
            writingText = true;
            erasingText = false;
        }
    }

    void DisableMarker()
    {
        lookingAtIt = false;
        animator.SetBool("MarkerVisable", false);


        if (ShouldHaveText)
        {
            if (characterIndex > 0)
            {
                writingText = false;
                erasingText = true;
            }
        }
    }

    void WriteText()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += timePerCharacter;
            characterIndex++;
            uiText.text = textToWrite.Substring(0, characterIndex);

            if (characterIndex >= textToWrite.Length)
            {
                writingText = false;
                Debug.Log("Done Writing");
            }
        }
    }

    void EraseText()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += timePerRemoval;
            characterIndex--;
            uiText.text = textToWrite.Substring(0, characterIndex);
            if (characterIndex <= 0)
            {
                erasingText = false;
                Debug.Log("Done Erasing");
            }
        }
    }


}
