using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class PlayerSoundHandler : MonoBehaviour
{
    AudioSource audSource;
    SkeletonAnimation skelAnim;

    [SerializeField] AudioClip[] clips;
    //0 = footstep

    private void Start()
    {
        audSource = GetComponent<AudioSource>();
        skelAnim = GetComponentInChildren<SkeletonAnimation>();

        skelAnim.state.Event += StateEvent;
    }

    private void StateEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "footsteps")
        {
            PlaySound(clips[0]);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audSource.pitch = Random.Range(0.8f, 1.2f);
        audSource.PlayOneShot(clip);
    }
}
