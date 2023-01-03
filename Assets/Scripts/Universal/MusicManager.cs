using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] AudioClip menuIntro;
    [SerializeField] AudioClip menuBody;

    [Header("Game")]
    [SerializeField] AudioClip gameIntro;
    [SerializeField] AudioClip gameBody;

    public enum GameScene { Menu, Game }
    public GameScene currentScene;

    AudioSource source;

    [HideInInspector] public float musicPitch = 1;

    private void Awake()
    {
        currentScene = GameScene.Menu;
        source = GetComponent<AudioSource>();
        musicPitch = 1;

        source.clip = menuIntro;
        source.loop = false;
        source.Play();
    }

    private void Update()
    {
        source.pitch = musicPitch;

        if (source.clip == menuIntro && source.isPlaying == false)
        {
            UpdateClip(GameScene.Menu, true);
        }
        if(source.clip == gameIntro && source.isPlaying == false)
        {
            UpdateClip(GameScene.Game, true);
        }
    }

    public void UpdateClip(GameScene scene, bool body) //are we in the menu or game, and has the intro played?
    {
        currentScene = scene;

        switch (currentScene)
        {
            case GameScene.Menu:
                if (!body)
                {
                    source.clip = menuIntro;
                    source.loop = false;
                    source.Play();
                }
                else if (body)
                {
                    source.clip = menuBody;
                    source.loop = true;
                    source.Play();
                }
                break;
            case GameScene.Game:
                if (!body)
                {
                    source.clip = gameIntro;
                    source.loop = false;
                    source.Play();
                }
                else if (body)
                {
                    source.clip = gameBody;
                    source.loop = true;
                    source.Play();
                }
                break;
            default:
                break;
        }
    }
}
