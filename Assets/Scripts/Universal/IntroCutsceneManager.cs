using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutsceneManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject introCutsceneMarker;
    [SerializeField] GameObject[] healthbar;
    [SerializeField] GameObject player;
    [SerializeField] GameObject firstMarker;

    [Header("Audio")]
    [SerializeField] AudioSource rainSound;
    [SerializeField] float rainVolume;
    [SerializeField] float volumeMultiplier;
    [SerializeField] float musicPitchSpeed = .5f;

    private void Start()
    {
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

    public void IntroCutsceneDone()
    {
        for (int i = 0; i < healthbar.Length; i++) { healthbar[i].SetActive(true); }

        //player.SetActive(false);
        player.GetComponent<PlayerInputHandler>().enabled = true;

        if (PlayerPrefs.GetInt("ShowHints") == 1)
            firstMarker.SetActive(true);
    }

    private void Update()
    {
        MusicManager music = GameObject.Find("Music").GetComponent<MusicManager>();
        if(music != null && music.musicPitch < 1)
        {
            music.musicPitch += Time.deltaTime * musicPitchSpeed;
        }
        else if(music.musicPitch > 1) { music.musicPitch = 1; }

        if (rainSound.volume <= 1 && rainVolume <= 1)
        {
            rainVolume += Time.deltaTime * volumeMultiplier;

            rainSound.volume = rainVolume;
        }
    }
}
