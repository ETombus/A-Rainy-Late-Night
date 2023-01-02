using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineEventHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip[] attackClips;
    [Tooltip("0 = no roof, 1 = roof")]
    [SerializeField] AudioClip[] jumpClips, landClips, stepClips;

    [Header("Particles")]
    [Tooltip("0 = no roof, 1 = roof")]
    [SerializeField] ParticleSystem[] jumpParticles;
    [Tooltip("0 = no roof, 1 = roof")]
    [SerializeField] ParticleSystem[] landParticles, stepParticles;

    [SerializeField] PlayerSoundHandler soundHandler;
    private UmbrellaStateHandler umbrellaHandler;
    private Vector2 footPos;



    private void Start()
    {
        umbrellaHandler = GetComponentInChildren<UmbrellaStateHandler>();
    }

    public void Attack()
    {

    }

    public void Jump()
    {
        Debug.Log("jump");
        WetDryCheck(jumpParticles, jumpClips, footPos);
    }

    public void Land()
    {
        Debug.Log("land");
        WetDryCheck(landParticles, landClips, footPos);
    }

    public void Step()
    {
        Debug.Log("step");
        if (!soundHandler.audSource.isPlaying)
        {
            WetDryCheck(stepParticles, stepClips, footPos);
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
