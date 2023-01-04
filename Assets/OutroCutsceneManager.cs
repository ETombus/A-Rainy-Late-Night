using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroCutsceneManager : MonoBehaviour
{
    [Header("Cutscene")]
    [SerializeField] bool cutsceneStarted = false;
    [SerializeField] GameObject overlayPanel;
    [SerializeField] float timeBeforeStopWalking;

    //Components
    GameObject player;
    Rigidbody2D playerRBody;
    PlayerStateHandler playerHandler;
    Walking walking;
    MusicManager music;
    PauseManager pauseManager;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerRBody = player.GetComponent<Rigidbody2D>();
        playerHandler = player.GetComponent<PlayerStateHandler>();
        walking = player.GetComponent<Walking>();
        pauseManager = GameObject.Find("PausePackage").GetComponent<PauseManager>();

        GameObject musicManager = GameObject.Find("Music");
        if (musicManager != null)
            music = musicManager.GetComponent<MusicManager>();

        cutsceneStarted = false;
    }

    private void Update()
    {
        if (cutsceneStarted)
        {
            timeBeforeStopWalking -= Time.deltaTime;
        }
        if (timeBeforeStopWalking <= 0)
        {
            playerRBody.velocity = Vector2.zero;
            playerHandler.inputX = 0;
            walking.UpdateCurrentVelocity();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInputHandler>().enabled = false;
            playerHandler.inputX = 1;

            if (music != null)
                music.FadeClip(MusicManager.GameScene.Outro, true);

            pauseManager.enabled = false;

            cutsceneStarted = true;

            overlayPanel.SetActive(true);
        }
    }
}
