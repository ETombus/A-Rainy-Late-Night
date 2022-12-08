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

    [Header("Aim variables")]
    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;

    public float aimOffset = 1;

    private Bone aimBone;

    // Start is called before the first frame update
    void Start()
    {
        handler = GetComponentInParent<EnemyHandler>();
        skelAnimation = GetComponent<SkeletonAnimation>();
        previousState = Mode.Idle;
        aimBone = skelAnimation.Skeleton.FindBone(boneName);


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
        //UpdateTargetLocation();
        previousState = currentState;
    }

    //void UpdateTargetLocation()
    //{
    //    Vector3 direction = handler.playerTrans.position - transform.position;
    //    direction.Normalize();

    //    var skeletonSpacePoint = skelAnimation.transform.InverseTransformPoint(direction);
    //    skeletonSpacePoint.x *= skelAnimation.Skeleton.ScaleX;
    //    skeletonSpacePoint.y *= skelAnimation.Skeleton.ScaleY;
    //    aimBone.SetLocalPosition(handler.playerTrans.position);
    //}

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

    private void OnDrawGizmos()
    {
        if (handler != null)
            Gizmos.DrawWireSphere( transform.position + new Vector3(0, 1.8f) + ((handler.playerTrans.position - (transform.position + new Vector3(0, 1.8f))).normalized) * aimOffset, 0.3f);
    }
}
