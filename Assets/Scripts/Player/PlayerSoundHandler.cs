using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    public AudioSource audSource;
    private readonly Queue<AudioClip> clipQueue = new();

    private void Start()
    {
        audSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (clipQueue.Count > 0 && !audSource.isPlaying)
        {
            var clip = clipQueue.Dequeue();
            PlaySound(clip);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audSource.pitch = Random.Range(0.8f, 1.2f);
        audSource.volume = 1;
        audSource.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        audSource.pitch = Random.Range(0.8f, 1.2f);
        audSource.volume = 1;
        audSource.PlayOneShot(clip);
    }

    public void QueueSound(AudioClip clip)
    {
        clipQueue.Enqueue(clip);
    }
}
