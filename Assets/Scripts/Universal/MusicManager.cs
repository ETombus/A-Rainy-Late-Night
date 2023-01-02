using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip mainBody;

    AudioSource source;

    [HideInInspector] public float musicPitch = 1;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        musicPitch = 1;

        source.clip = intro;
        source.loop = false;
        source.Play();
    }

    private void Update()
    {
        source.pitch = musicPitch;

        if (source.clip == intro && source.isPlaying == false)
        {
            source.clip = mainBody;
            source.loop = true;
            source.Play();
        }
    }
}
