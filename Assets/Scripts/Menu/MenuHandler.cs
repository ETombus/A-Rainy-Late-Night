using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject[] panels; //0 = main panel, 1 = settingspanel, 2 = tutorial, 3 = credits
    [SerializeField] int panelIndex = 0;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject creditsPanel;

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
        if (PlayerPrefs.GetInt("PlayIntroAnimation") == 1)
        {
            fadeIn.SetActive(false);
            introMarker.SetActive(false);
        }
    }

    public void ButtonStart()
    {
        Debug.Log("Game Started");
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("PlayIntroAnimation", 1); //1 = false, 0 = true
    }

    public void ButtonSettings()
    {
        SetActivePanel(1);
        panelIndex = 1;
    }

    public void ButtonTutorial()
    {
        SetActivePanel(2);
        panelIndex = 2;
    }

    public void ButtonCredits()
    {
        SetActivePanel(3);
        panelIndex = 3;
    }

    public void ButtonExit()
    {
        Debug.Log("Exited Game");
        Application.Quit();
        PlayerPrefs.DeleteKey("PlayIntroAnimation"); //1 = false, 0 = true
    }

    public void ButtonLeft()
    {
        if (panelIndex > 0)
            panelIndex--;

        SetActivePanel(panelIndex);
    }

    public void ButtonRight()
    {
        if (panelIndex < panels.Length - 1)
            panelIndex++;

        SetActivePanel(panelIndex);
    }

    void SetActivePanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        panels[index].SetActive(true);
    }
}
