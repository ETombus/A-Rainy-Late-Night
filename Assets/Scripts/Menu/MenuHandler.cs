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

    [Header("IntroCutscene")]
    [SerializeField] GameObject introCutscene;
    public bool allowSceneLoad = false;
    [HideInInspector] public float rainVolume = 1;
    [SerializeField] AudioClip falling;
    [SerializeField] AudioClip fallThump;

    [Header("Audio")]
    [SerializeField] AudioClip pageTurn;
    [SerializeField] AudioSource[] audSource;
    MusicManager music;
    [SerializeField] float musicPitchSpeed = .3f; 

    PlayerInputs playerControls;
    InputAction cancel;

    private void Awake()
    {
        rainVolume = 1;
        music = GameObject.Find("Music").GetComponent<MusicManager>();

        playerControls = new PlayerInputs();
        if (PlayerPrefs.GetInt("PlayIntroAnimation") == 1)
        {
            fadeIn.SetActive(false);
            introMarker.SetActive(false);
        }

        //audSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        cancel = playerControls.UI.Cancel;
        cancel.Enable();

        cancel.performed += ButtonBack;
    }

    private void OnDisable() { cancel.Disable(); }

    private void Update()
    {
        audSource[1].volume = rainVolume;
    }

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

        PlaySoundEffect();
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

    public void PlayIntroCutscene()
    {
        StartCoroutine(Loadscene());
        introCutscene.SetActive(true);
        audSource[0].loop = true;
        audSource[0].clip = falling;
        audSource[0].Play();
    }

    IEnumerator Loadscene()
    {
        yield return null;

        //Begin loading scene
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(1);
        //dont allow to finish loading
        asyncOp.allowSceneActivation = false;
        Debug.Log("started loading scene");

        while (!asyncOp.isDone)
        {
            if (music.musicPitch >= 0.2f) { music.musicPitch -= Time.deltaTime * musicPitchSpeed; }

            Debug.Log("Currently loading, Pro : " + asyncOp.progress);

            if (asyncOp.progress >= 0.9f && allowSceneLoad)
            {
                //Debug.Log("progress is at: " + asyncOp.progress);
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public IEnumerator PlayThump()
    {
        yield return null;
        music.gameObject.GetComponent<AudioSource>().Pause();

        audSource[0].loop = false;
        audSource[0].volume = 1.5f;
        audSource[0].pitch = 1f;
        audSource[0].clip = fallThump;
        audSource[0].Play();

        while (audSource[0].isPlaying)
        {
            yield return null;
        }

        if (!audSource[0].isPlaying)
            allowSceneLoad = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("HighestLevelReached", 1));
        PlayerPrefs.SetInt("PlayIntroAnimation", 1); //1 = false, 0 = true
    }

    public void ResetCheckpoints()
    {
        PlayerPrefs.DeleteKey("CheckpointReached");
        PlayerPrefs.SetInt("PlayIntroCutscene", 1);
    }

    public void PregameBack()
    {
        preGamePanel.SetActive(false);
    }

    void PlaySoundEffect()
    {
        audSource[0].pitch = Random.Range(0.8f, 1.2f);
        audSource[0].PlayOneShot(pageTurn);
    }
}
