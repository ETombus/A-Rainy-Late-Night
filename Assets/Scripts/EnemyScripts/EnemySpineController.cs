using System.Collections;
using UnityEngine;
using Spine;
using Spine.Unity;
using static EnemyHandler;

public class EnemySpineController : MonoBehaviour
{
    [SerializeField] AnimationReferenceAsset idle, run;
    SkeletonAnimation skelAnimation;

    EnemyHandler handler;
    Mode previousState;

    // Start is called before the first frame update
    void Start()
    {
        handler = GetComponentInParent<EnemyHandler>();
        skelAnimation = GetComponent<SkeletonAnimation>();

        if (handler == null) Debug.LogError("Handler in " + gameObject.name + " is missing");
    }

    // Update is called once per frame
    void Update()
    {
        var currentState = handler.currentMode;
        if (previousState != currentState)
        {
            PlayNewAnimation();
        }
        currentState = previousState;
    }

    void PlayNewAnimation()
    {
        var newAnimationState = handler.currentMode;
        Spine.Animation nextAnimation;

        switch (newAnimationState)
        {
            case Mode.Patrol:
                nextAnimation = run;
                skelAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
                break;
            case Mode.Aggression:
                nextAnimation = idle;
                skelAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
                break;
            case Mode.Search:
                nextAnimation = run;
                skelAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
                break;
            case Mode.Idle:
                nextAnimation = idle;
                skelAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
                break;
            default:
                Debug.LogError("Invalid Animation State");
                break;
        }
    }
}
