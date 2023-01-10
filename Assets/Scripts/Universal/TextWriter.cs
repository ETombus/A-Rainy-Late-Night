using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    float timer;
    int characterIndex = 0;
    [SerializeField] string textToWrite;

    TextMeshProUGUI uiText;

    bool writingText;
    [SerializeField] float timePerCharacter = 0.1f;

    private void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        uiText.text = " ";
    }

    private void Update()
    {
        if (writingText) { WriteText(); }
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
            }
        }
    }

    public void StartWriting()
    {
        writingText = true;
    }
}
