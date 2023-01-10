using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScannerController : MonoBehaviour
{
    public static ScannerController Instance;

    public TextMeshProUGUI uiText;
    public GameObject Player;

    public float timePerRemoval = 0.05f;
    public float timeBeforeTextRemoval = 1;

    [SerializeField] bool currentlyWriting = false;
    [SerializeField] bool currentlyErasing = false;

    string textToWrite;
    float timePerCharacter;

    string nexttTextToWrite;
    float nextTimePerCharacter;


    int characterIndex;
    float timer;

    // Activate text

    private void Awake()
    {
        if (ScannerController.Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (Player == null)
            Player = GameObject.FindWithTag("Player");

        if (uiText != null)
            uiText.text = "";
    }


    public void WriteText(string textToWrite, float timePerCharacter)
    {
        if (!currentlyWriting && !currentlyErasing)
        {
            if (characterIndex == 0)
            {
                this.textToWrite = textToWrite;
                this.timePerCharacter = timePerCharacter;
                currentlyWriting = true;
            }
            else
                RemoveText();
        }
    }

    private void Update()
    {

        if (uiText != null && currentlyWriting && !currentlyErasing)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer += timePerCharacter;
                characterIndex++;
                uiText.text = textToWrite.Substring(0, characterIndex);

                if (characterIndex >= textToWrite.Length)
                {
                    currentlyWriting = false;
                    Debug.Log("Done Writing");

                    Invoke(nameof(RemoveText), timeBeforeTextRemoval);
                }
            }
        }
        else if (uiText != null && currentlyErasing)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer += timePerRemoval;
                characterIndex--;
                uiText.text = textToWrite.Substring(0, characterIndex);
                if (characterIndex <= 0)
                {
                    currentlyErasing = false;
                    Debug.Log("Done Erasing");
                }
            }
        }
    }

    private void RemoveText()
    {
        currentlyWriting = false;
        currentlyErasing = true;
        CancelInvoke();
    }

}
