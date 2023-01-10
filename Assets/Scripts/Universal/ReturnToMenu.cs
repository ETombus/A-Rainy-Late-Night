using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    MusicManager music;

    public float musicVolume;

    private void Start()
    {
        GameObject musicManager = GameObject.Find("Music");
        if (musicManager != null)
            music = musicManager.GetComponent<MusicManager>();
    }

    private void Update()
    {
        music.musicVolume = musicVolume;
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void FadeOutMusic()
    {
        if (music != null)
            music.FadeClip(MusicManager.GameScene.Outro, false);
    }
}
