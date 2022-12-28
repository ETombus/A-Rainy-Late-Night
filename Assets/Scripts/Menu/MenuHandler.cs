using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuHandler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject[] panels; //0 = main panel, 1 = settingspanel, 2 = tutorial, 3 = credits
    [SerializeField] int panelIndex = 0;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject preGamePanel;

    [Header("Intro")]
    [SerializeField] GameObject fadeIn;
    [SerializeField] GameObject introMarker;


    PlayerInputs playerControls;
    InputAction cancel;

    private void Awake()
    {
        playerControls = new PlayerInputs();
        if (PlayerPrefs.GetInt("PlayIntroAnimation") == 1)
        {
            fadeIn.SetActive(false);
            introMarker.SetActive(false);
        }
    }
    private void OnEnable()
    {
        cancel = playerControls.UI.Cancel;
        cancel.Enable();

        cancel.performed += ButtonBack;
    }

    private void OnDisable() { cancel.Disable(); }

    public void ButtonStart()
    {
        preGamePanel.SetActive(true);
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

    public void ButtonBack(InputAction.CallbackContext context)
    {
        SetActivePanel(0);
        panelIndex = 0;
    }
    public void ButtonBack()
    {
        SetActivePanel(0);
        panelIndex = 0;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("HighestLevelReached", 1));
        PlayerPrefs.SetInt("PlayIntroAnimation", 1); //1 = false, 0 = true
    }

    public void ResetCheckpoints()
    {
        PlayerPrefs.DeleteKey("CheckpointReached");
    }

    public void PregameBack()
    {
        preGamePanel.SetActive(false);
    }
}
