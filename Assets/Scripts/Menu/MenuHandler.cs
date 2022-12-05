using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject settingsPanel;

    [Header("Variables")]
    [SerializeField] float speed = 50;

    [SerializeField] GameObject fadeIn;
    [SerializeField] GameObject introMarker;

    bool moveCameraUp;
    bool showSettings;

    Vector3 offset = new(0, 0, -10);

    bool playIntroAnimation = true;

    private void Start()
    {
        if(PlayerPrefs.GetInt("PlayIntroAnimation") == 1)
        {
            fadeIn.SetActive(false);
            introMarker.SetActive(false);
        }
    }

    private void Update()
    {
        if (moveCameraUp)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, tutorialPanel.transform.position + offset, speed * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, Vector3.zero + offset, speed * Time.deltaTime);
        }

        if (showSettings)
        {
            mainPanel.transform.position = Vector3.MoveTowards(mainPanel.transform.position, new(5, 0, 0), speed * Time.deltaTime);
        }
        else
        {
            mainPanel.transform.position = Vector3.MoveTowards(mainPanel.transform.position, Vector3.zero, speed * Time.deltaTime);
        }
    }

    public void ButtonStart()
    {
        Debug.Log("Game Started");
        SceneManager.LoadScene(1); 
        PlayerPrefs.SetInt("PlayIntroAnimation", 1); //1 = false, 0 = true
    }

    public void ButtonTutorial()
    {
        //tutorialPanel.SetActive(true);
        moveCameraUp = true;
    }

    public void ButtonCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void ButtonSettings()
    {
        showSettings = true;
        settingsPanel.SetActive(true);
    }

    public void ButtonExit()
    {
        Debug.Log("Exited Game");
        Application.Quit();
        PlayerPrefs.DeleteKey("PlayIntroAnimation"); //1 = false, 0 = true
    }

    public void BackTutorial()
    {
        moveCameraUp = false;
    }

    public void BackCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void BackSettings()
    {
        showSettings = false;
        settingsPanel.SetActive(false);
    }
}
