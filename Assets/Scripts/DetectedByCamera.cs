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
    public float distanceFromPlayer = 100;

    [Header("Hook Variables")]
    public bool isAHookPoint = false;
    public bool isSelected = false;

    [Header("Text Varibles")]
    public bool ShouldHaveText = false;
    public string textToWrite;

    public float timePerCharacter = 0.2f;
    public float timePerRemoval = 0.05f;

    private TextMeshProUGUI uiText;
    private Canvas canvas;

    int characterIndex;
    float timer;

    private bool writingText = false;
    private bool erasingText = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>();

        if (ShouldHaveText)
        {
            canvas.enabled = true;
            uiText = GetComponentInChildren<TextMeshProUGUI>();
            uiText.text = "";
        }
        else
        {
            canvas.enabled = false;
        }

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

        if(isAHookPoint)
        {
            if (isSelected)
                animator.SetBool("Selected", true);
            else
                animator.SetBool("Selected", false);
        }


        if (writingText)
            WriteText();
        else if (erasingText)
            EraseText();
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

        if (ShouldHaveText)
        {
            writingText = true;
            erasingText = false;
        }
    }

    void DisableMarker()
    {
        lookingAtIt = false;
        animator.SetTrigger("RemoveMarker");

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
