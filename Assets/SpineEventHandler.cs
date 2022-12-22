using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineEventHandler : MonoBehaviour
{
    [SerializeField] AudioClip step;
    [SerializeField] ParticleSystem footsplash;


    [SerializeField] PlayerSoundHandler soundHandler;

    public void Footstep()
    {
        footsplash.Play();
        soundHandler.PlaySound(step);
    }
}
