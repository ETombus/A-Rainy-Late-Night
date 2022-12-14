using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineEventHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip[] attackClips;
    [Tooltip("0 = wet, 1 = dry")]
    [SerializeField] AudioClip[] jumpClips, landClips, stepClips;

    [Header("Particles")]
    [Tooltip("0 = wet, 1 = dry")]
    [SerializeField] ParticleSystem[] jumpParticles;
    [Tooltip("0 = wet, 1 = dry")]
    [SerializeField] ParticleSystem[] landParticles, stepParticles;

    [SerializeField] PlayerSoundHandler soundHandler;
    private UmbrellaStateHandler umbrellaHandler;
    [SerializeField] Transform footPos;



    private void Start()
    {
        umbrellaHandler = GetComponentInChildren<UmbrellaStateHandler>();
    }

    //public void Attack()
    //{
    //    Debug.Log("attack");
    //    if (attackClips != null)
    //        soundHandler.PlaySound(attackClips[Random.Range(0,attackClips.Length)]);
    //}

    public void Jump()
    {
        WetDryCheck(jumpParticles, jumpClips, footPos.position);
    }

    public void Land()
    {
        WetDryCheck(landParticles, landClips, footPos.position);
    }

    public void Step()
    {
        if (!soundHandler.audSource.isPlaying)
        {
            WetDryCheck(stepParticles, stepClips, footPos.position);
        }
    }

    private void WetDryCheck(ParticleSystem[] particles, AudioClip[] clips, Vector2 pos)
    {
        if (!umbrellaHandler.roof)
        {
            if (clips != null)
                soundHandler.PlaySound(clips[0]);

            if (particles != null)
            {
                var particle = Instantiate(particles[0], pos, Quaternion.identity);
                Destroy(particle, 1f);
            }
        }
        else
        {
            if (clips != null)
                soundHandler.PlaySound(clips[1]);

            if (particles != null)
            {
                var particle = Instantiate(particles[1], pos, Quaternion.identity);
                Destroy(particle, 1f);

            }
        }
    }
}
