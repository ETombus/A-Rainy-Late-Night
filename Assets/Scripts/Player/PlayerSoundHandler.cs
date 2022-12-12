using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    AudioSource audSource;

    private void Start()
    {
        audSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audSource.pitch = Random.Range(0.8f, 1.2f);
        audSource.PlayOneShot(clip);
    }
}
