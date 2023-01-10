using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator anim;
    public string animationTriggerName;

    public Sprite newSprite;



    public void PlayAnimationClip()
    {
        try
        {
        anim = GetComponent<Animator>();
        anim.SetTrigger(animationTriggerName);

        }
        catch
        {
            Debug.LogWarning("Warning: Missing Animator on " + gameObject);
        }
    }

    public void ChangeSprite()
    {
        try
        {
           gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
        catch
        {
            Debug.LogWarning("Warning: Missing SpriteRenderer or Sprite on " + gameObject);
        }
    }

}
