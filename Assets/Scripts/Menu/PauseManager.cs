using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject youSurePanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject checkpointsPanel;
    [SerializeField] GameObject settingsPanel;

    PlayerInputs playerControls;
    private InputAction escape;

    GameObject player;

    CheckpointManager checkpoints;
    GameOverManager gameOver;

    [Header("Main Panel")]
    [SerializeField] Toggle skipDeathToggle;

    [Header("Settings")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider enviromentSlider;
    [SerializeField] Toggle muteMusicToggle;
    [SerializeField] Toggle muteSoundToggle;

    bool isPaused;

    private void Awake()
    {
        playerControls = new PlayerInputs();

        checkpoints = GetComponent<CheckpointManager>();
        if (checkpoints == null) { checkpoints = GameObject.Find("GameManager").GetComponent<CheckpointManager>(); }
        gameOver = GameObject.Find("GameManager").GetComponent<GameOverManager>();

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
        else if (isPaused && tutorialPanel.activeSelf == false) { UnpauseGame(); }
        else if (isPaused && tutorialPanel.activeSelf == true) { TutorialClose(); }
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

    public void SkipDeathPanel(bool skip)
    {
        gameOver.SkipDeathScreen(skip);
    }

    public void ToggleSettings(bool toggle)
    {
        if (toggle)
        {
            pausePanel.SetActive(false);
            settingsPanel.SetActive(true);

            SetSliders();
        }
        else
        {
            settingsPanel.SetActive(false);
            pausePanel.SetActive(true);
        }
    }

    void SetSliders()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        enviromentSlider.value = PlayerPrefs.GetFloat("EnviromentVolume", 0.40f);

        if (PlayerPrefs.GetFloat("MusicMuted") == 0) { muteMusicToggle.isOn = false; }
        else
        {
            muteMusicToggle.isOn = true;
            MuteMusic(true);
        }

        if (PlayerPrefs.GetFloat("SoundMuted") == 0) { muteSoundToggle.isOn = false; }
        else
        {
            muteSoundToggle.isOn = true;
            MuteSound(true);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVol", ConvertLog(sliderValue));
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        //Debug.Log("sliderValue = " + sliderValue);
    }

    public void SetSoundVolume(float sliderValue)
    {
        mixer.SetFloat("SoundVol", ConvertLog(sliderValue));
        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
    }

    public void SetEnviromentVolume(float sliderValue)
    {
        mixer.SetFloat("EnviromentVol", ConvertLog(sliderValue));
        PlayerPrefs.SetFloat("EnviromentVolume", sliderValue);
    }

    float ConvertLog(float value)
    {
        float convertedValue = Mathf.Log10(value) * 20;
        return convertedValue;
    }

    public void MuteMusic(bool isMuted)
    {
        if (isMuted)
        {
            musicSlider.interactable = false;
            mixer.SetFloat("MusicVol", -80);
            PlayerPrefs.SetFloat("MusicMuted", 1);
        }
        else
        {
            musicSlider.interactable = true;
            mixer.SetFloat("MusicVol", ConvertLog(musicSlider.value));
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
            PlayerPrefs.SetFloat("MusicMuted", 0);
        }
    }

    public void MuteSound(bool isMuted)
    {
        if (isMuted)
        {
            soundSlider.interactable = false;
            enviromentSlider.interactable = false;
            mixer.SetFloat("SoundVol", -80);
            mixer.SetFloat("EnviromentVol", -80);
            PlayerPrefs.SetFloat("SoundMuted", 1);
        }
        else
        {
            soundSlider.interactable = true;
            enviromentSlider.interactable = true;
            mixer.SetFloat("SoundVol", ConvertLog(soundSlider.value));
            mixer.SetFloat("EnviromentVol", ConvertLog(enviromentSlider.value));
            PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
            PlayerPrefs.SetFloat("EnviromentVolume", soundSlider.value);
            PlayerPrefs.SetFloat("SoundMuted", 0);
        }
    }

}
