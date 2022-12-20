using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayAnimation : MonoBehaviour
{
    private Animator anim;
    public string animationTriggerName;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void PlayAnimationClip()
    {
        anim.SetTrigger(animationTriggerName);
    }

}
