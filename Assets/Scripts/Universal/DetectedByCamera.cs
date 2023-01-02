using Spine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DetectedByCamera : MonoBehaviour
{
    Animator animator;
    private AudioSource audSource;

    [Header("Sound")]
    public AudioClip detectionSound;
    [SerializeField] bool playSound;

    bool lookingAtIt = false;
    bool disabled = false;

    [Range(0f, 185f)]
    public float maxDistanceFromPlayer = 100;

    [Range(0f, 185f)]
    public float minDistanceFromPlayer = 0;

    public Transform Target;

    [Header("Hook Variables")]
    public bool isAHookPoint = false;
    public bool isSelected = false;

    [Header("Text Varibles")]
    [Tooltip("Writes the text in text to write as long as there is an attached child canvas")]
    public bool ShouldHaveText = false;
    public string textToWrite;

    public float timePerCharacter = 0.2f;
    public float timePerRemoval = 0.05f;

    [Header("Image")]
    [SerializeField] bool shouldHaveImage;

    [Header("Misc")]
    public bool showRadius = false;
    [SerializeField] bool disableUponFinish = false;

    [Header("Disabling")]
    [SerializeField] float timeBeforeDisappearing = 2f;
    [SerializeField] float disappearingTimer;

    private Image uiImage;
    private TextMeshProUGUI uiText;
    public GameObject canvas;

    int characterIndex;
    float timer;

    private bool writingText = false;
    private bool erasingText = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audSource = GetComponent<AudioSource>();

        //if(writingSound != null)
        //audSource.clip = writingSound;

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
        if (shouldHaveImage)
        {
            canvas.SetActive(true);
            uiImage = GetComponentInChildren<Image>();
            uiImage.gameObject.SetActive(false);
        }
        else if (!shouldHaveImage && !ShouldHaveText)
        {
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }

    bool toggleSound;

    public void ToggleMarker()
    {
        if (!disabled)
            disabled = true;
        else
            disabled = false;
    }

    void Update()
    {
        if (CloseEnoughToPlayer(true) && !lookingAtIt && !disabled)
        {
            ActivateMarker();

            if (shouldHaveImage)
                uiImage.gameObject.SetActive(true);
        }
        else if ((!CloseEnoughToPlayer(true) && lookingAtIt) || disabled)
        {
            animator.SetBool("MarkerVisable", false);
        }

        if (disappearingTimer > 0 && !CloseEnoughToPlayer(true)) { disappearingTimer -= Time.deltaTime; }
        else if (disappearingTimer <= 0 && !writingText)
        {
            DisableMarker();

            if (shouldHaveImage)
            {
                uiImage.gameObject.SetActive(false);
            }
        }


        if (isAHookPoint)
        {
            if (isSelected && CloseEnoughToPlayer(false))
                animator.SetBool("Selected", true);
            else
                animator.SetBool("Selected", false);
        }

        //if(writingText && toggleSound && writingSound != null)
        //{
        //    audSource.Play();
        //    toggleSound = false;
        //}

        if (writingText)
            WriteText();
        else if (erasingText)
            EraseText();
    }

    float markerToPlayer;

    public bool CloseEnoughToPlayer(bool returnMax)
    {
        //Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        //return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
        if (Target != null)
            markerToPlayer = (Target.position - ScannerController.Instance.Player.transform.position).sqrMagnitude;
        else
            markerToPlayer = (transform.position - ScannerController.Instance.Player.transform.position).sqrMagnitude;

        //Whole Screen is ca 185 units

        if (returnMax)
            return markerToPlayer < maxDistanceFromPlayer;
        else
            return markerToPlayer > minDistanceFromPlayer;
    }

    void ActivateMarker()
    {
        if (detectionSound != null)
            audSource.PlayOneShot(detectionSound);

        lookingAtIt = true;
        animator.SetBool("MarkerVisable", true);


        if (ShouldHaveText)
        {
            writingText = true;
            erasingText = false;

            //if (!toggleSound)
            //{
            //    audSource.Stop();
            //    toggleSound = true;
            //}
        }
    }

    void DisableMarker()
    {
        lookingAtIt = false;

        if (ShouldHaveText)
        {
            if (characterIndex > 0)
            {
                writingText = false;
                erasingText = true;

                //if (!toggleSound)
                //{
                //    audSource.Stop();
                //    toggleSound = true;
                //}
            }
        }
    }

    void WriteText()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && characterIndex < textToWrite.Length)
        {
            timer += timePerCharacter;
            characterIndex++;
            uiText.text = textToWrite.Substring(0, characterIndex);

            if (characterIndex >= textToWrite.Length)
            {
                writingText = false;
                disappearingTimer = timeBeforeDisappearing;
                if (disableUponFinish)
                {
                    IntroCutsceneManager cutsceneManager = GameObject.Find("GameManager").GetComponent<IntroCutsceneManager>();
                    cutsceneManager.IntroCutsceneDone();
                } //TODO maybe change this to a fadeoutAnimation
            }
        }
    }

    void EraseText()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && characterIndex > 0)
        {
            timer += timePerRemoval;
            characterIndex--;
            uiText.text = textToWrite.Substring(0, characterIndex);
            if (characterIndex <= 0)
            {
                erasingText = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showRadius)
        {
            Gizmos.color = Color.yellow;
            if (Target != null)
                Gizmos.DrawWireSphere(Target.position, Mathf.Sqrt(maxDistanceFromPlayer));
            else
                Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(maxDistanceFromPlayer));
        }
    }
}
