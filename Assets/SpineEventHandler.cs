using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineEventHandler : MonoBehaviour
{
    [SerializeField] AudioClip[] step;
    [SerializeField] ParticleSystem footsplash;
    private UmbrellaStateHandler umbrellaHandler;

    [SerializeField] PlayerSoundHandler soundHandler;

    private void Start()
    {
        umbrellaHandler = GetComponentInChildren<UmbrellaStateHandler>();
    }

    public void Footstep()
    {
        if (!soundHandler.audSource.isPlaying)
        {
            if (!umbrellaHandler.roof)
            {
                footsplash.Play();
                soundHandler.PlaySound(step[0]);
            }
            else
            {
                soundHandler.PlaySound(step[1]);
            }
        }
    }
}
