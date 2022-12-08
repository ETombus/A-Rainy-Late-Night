using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject youSurePanel;

    PlayerInputs playerControls;
    private InputAction escape;

    [SerializeField] GameObject player;

    private void Awake()
    {
        playerControls = new PlayerInputs();
    }

    private void OnEnable()
    {
        escape = playerControls.UI.Escape;
        escape.Enable();
        escape.performed += PauseGame;
    }

    private void OnDisable() { escape.Disable(); }

    private void PauseGame(InputAction.CallbackContext context)
    {
        EnablePlayerActions(false);

        Debug.Log("Game Paused");
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void UnpauseGame()
    {
        EnablePlayerActions(true);

        Time.timeScale = 1;
        pausePanel.SetActive(false);
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
        player.GetComponent<Slice>().enabled = setTo;
        player.GetComponentInChildren<AimTowardsMouse>().enabled = setTo;
    }
}
