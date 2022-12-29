using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject youSurePanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject checkpointsPanel;

    PlayerInputs playerControls;
    private InputAction escape;

    GameObject player;

    CheckpointManager checkpoints;

    [SerializeField] Toggle skipDeathToggle;

    bool isPaused;

    private void Awake()
    {
        playerControls = new PlayerInputs();

        checkpoints = GetComponent<CheckpointManager>();
        if (checkpoints == null) { checkpoints = GameObject.Find("GameManager").GetComponent<CheckpointManager>(); }

        player = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        escape = playerControls.UI.Escape;
        escape.Enable();
        escape.performed += EscapePressed;
    }

    private void OnDisable() { escape.Disable(); }

    private void EscapePressed(InputAction.CallbackContext context)
    {
        if (!isPaused) { PauseGame(); }
        else if(isPaused && tutorialPanel.activeSelf == false) { UnpauseGame(); }
        else if(isPaused && tutorialPanel.activeSelf == true) { TutorialClose(); }
    }

    private void PauseGame()
    {
        EnablePlayerActions(false);

        Time.timeScale = 0;
        pausePanel.SetActive(true);

        if (PlayerPrefs.GetInt("SkipDeathScene") == 0) { skipDeathToggle.isOn = false; }
        else { skipDeathToggle.isOn = true; }
        isPaused = true;
    }

    public void UnpauseGame()
    {
        EnablePlayerActions(true);

        Time.timeScale = 1;
        pausePanel.SetActive(false);
        isPaused = false;
    }

    public void ReturnToMenu()
    {
        EnablePlayerActions(true);

        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ButtonExit()
    {
        youSurePanel.SetActive(true);
    }
    public void ButtonYouSureBack()
    {
        youSurePanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Game Quit");
        PlayerPrefs.SetInt("PlayIntroAnimation", 0); //0 = true, 1 = false
        Application.Quit();
    }

    private void EnablePlayerActions(bool setTo)
    {
        player.GetComponent<PlayerInputHandler>().enabled = setTo;
        //player.GetComponent<Slice>().enabled = setTo;
        //player.GetComponentInChildren<AimTowardsMouse>().enabled = setTo;
    }

    public void TutorialOpen()
    {
        tutorialPanel.SetActive(true);
    }

    public void OpenCheckpoints()
    {
        checkpointsPanel.SetActive(true);
    }

    public void CloseCheckpoints()
    {
        checkpointsPanel.SetActive(false);
    }

    public void TutorialClose()
    {
        tutorialPanel.SetActive(false);
    }

    public void LoadCheckpoint(int checkpointToLoad)
    {
        checkpoints.SetPlayerPosition(player, checkpointToLoad);
        checkpointsPanel.SetActive(false);
        UnpauseGame();
    }

    public void ResetCheckpoints()
    {
        PlayerPrefs.DeleteKey("CheckpointReached");
    }
}
