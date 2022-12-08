using System.Collections;
using UnityEngine;
using Spine;
using Spine.Unity;
using static EnemyHandler;

public class EnemySpineController : MonoBehaviour
{
    [SerializeField] AnimationReferenceAsset idle, run, walk, Aim;
    SkeletonAnimation skelAnimation;

    EnemyHandler handler;
    Mode previousState;

    // Start is called before the first frame update
    void Start()
    {
        handler = GetComponentInParent<EnemyHandler>();
        skelAnimation = GetComponent<SkeletonAnimation>();
        previousState = Mode.Idle;

        if (handler == null) Debug.LogError("Handler in " + gameObject.name + " is missing");
    }

    // Update is called once per frame
    void Update()
    {
        var currentState = handler.currentMode;
        Debug.Log(currentState);
        if (previousState != currentState)
        {
            PlayNewAnimation();
        }
        previousState = currentState;
    }

    void PlayNewAnimation()
    {
        var newAnimationState = handler.currentMode;
        Spine.Animation nextAnimation;

        switch (newAnimationState)
        {
            case Mode.Patrol:
                nextAnimation = walk;
                StopPlayingAim();
                break;
            case Mode.Aggression:
                nextAnimation = idle;
                PlayAim();
                break;
            case Mode.Search:
                nextAnimation = run;
                break;
            case Mode.Idle:
                nextAnimation = idle;
                StopPlayingAim();
                break;
            default:
                Debug.LogError("Invalid Animation State");
                nextAnimation = idle;
                break;
        }

        skelAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
    }


    void PlayAim()
    {
        var aimTrack = skelAnimation.AnimationState.SetAnimation(2, Aim, true);
        aimTrack.AttachmentThreshold = 1f;
        aimTrack.MixDuration = 0f;
    }
    public void StopPlayingAim()
    {
        skelAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);
    }

    void PlayShoot()
    {

    }
}
