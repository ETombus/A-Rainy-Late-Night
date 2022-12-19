using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip mainBody;

    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        source.clip = intro;
        source.loop = false;
        source.Play();
    }

    private void Update()
    {
        if (source.clip == intro && source.isPlaying == false)
        {
            source.clip = mainBody;
            source.loop = true;
            source.Play();
        }
    }
}
