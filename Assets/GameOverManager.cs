using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    CheckpointManager checkPoints;

    GameObject player;

    [SerializeField] GameObject gameOverPanel;

    [SerializeField] Toggle skipDeathToggle;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        checkPoints = GetComponent<CheckpointManager>();

        checkPoints.SetStartPos(player);
    }

    public void PlayerDeath()
    {
        if (PlayerPrefs.GetInt("SkipDeathScene") == 0)
        {
            Time.timeScale = 0;

            gameOverPanel.SetActive(true);

            skipDeathToggle.isOn = false;
        }
        else { ButtonRetry(); }
    }

    public void ButtonRetry()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);

        checkPoints.SetPlayerPosition(player);
    }

    public void ButtonReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SkipDeathScreen(bool skip) //0 = false, 1 = true
    {
        int skipInt = 0;
        if (skip == false) { skipInt = 0; }
        else if (skip == true) { skipInt = 1; }

        PlayerPrefs.SetInt("SkipDeathScene", skipInt);
    }
}
