using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroCutsceneManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject introCutsceneMarker;
    [SerializeField] GameObject[] healthbar;
    [SerializeField] GameObject player;
    [SerializeField] GameObject firstMarker;
    [SerializeField] GameObject clickToContinueText;
    [SerializeField] bool cutsceneDone;

    [Header("Audio")]
    [SerializeField] AudioSource rainSound;
    [SerializeField] float rainVolume;
    [SerializeField] float volumeMultiplier;
    [SerializeField] float musicPitchSpeed = .5f;

    PlayerInputs playerControls;
    InputAction mouseClick;
    MusicManager music;
    PauseManager pause;
    [SerializeField] Animator imageAnimator;
    [SerializeField] Animator textAnimator;

    private void Awake()
    {
        playerControls = new PlayerInputs();
    }

    private void Start()
    {
        music = GameObject.Find("Music").GetComponent<MusicManager>();
        music.UpdateClip(MusicManager.GameScene.Game, false);

        pause = GameObject.Find("PausePackage").GetComponent<PauseManager>();
        pause.enabled = false;


        if (PlayerPrefs.GetInt("PlayIntroCutscene", 1) == 1 && introCutsceneMarker.activeSelf == true)
        {
            for (int i = 0; i < healthbar.Length; i++) { healthbar[i].SetActive(false); }

            //player.SetActive(false);
            player.GetComponent<PlayerInputHandler>().enabled = false;
            firstMarker.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("PlayIntroCutscene") == 0)
        {
            introCutsceneMarker.SetActive(false);
            rainVolume = 1;
        }
    }

    private void OnEnable()
    {
        mouseClick = playerControls.UI.Click;
        mouseClick.Enable();

        mouseClick.performed += EndCutscene;
    }

    private void OnDisable() { mouseClick.Disable(); }

    public void IntroCutsceneDone()
    {
        clickToContinueText.SetActive(true);
        cutsceneDone = true;
    }

    public void EndCutscene(InputAction.CallbackContext context)
    {
        if (cutsceneDone)
        {
            clickToContinueText.SetActive(false);
            imageAnimator.SetTrigger("fadeOut");
            textAnimator.SetTrigger("fadeOut");
        }
    }

    public void BeginGame()
    {
        for (int i = 0; i < healthbar.Length; i++) { healthbar[i].SetActive(true); }

        //player.SetActive(false);
        player.GetComponent<PlayerInputHandler>().enabled = true;

        if (PlayerPrefs.GetInt("ShowHints") == 1)
            firstMarker.SetActive(true);


        PlayerPrefs.SetInt("PlayIntroCutscene", 0);
        introCutsceneMarker.SetActive(false);

        pause.enabled = true;

        OnDisable();
    }

    private void Update()
    {
        if (!music.gameObject.GetComponent<AudioSource>().isPlaying)
            music.gameObject.GetComponent<AudioSource>().Play();

        if (music != null && music.musicPitch < 1)
        {
            music.musicPitch += Time.deltaTime * musicPitchSpeed;
        }
        else if (music.musicPitch > 1) { music.musicPitch = 1; }

        if (rainSound.volume <= 1 && rainVolume <= 1)
        {
            rainVolume += Time.deltaTime * volumeMultiplier;

            rainSound.volume = rainVolume;
        }
    }
}
