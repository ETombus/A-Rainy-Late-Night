using System.Collections;
using UnityEngine;
using Spine;
using Spine.Unity;
using static EnemyHandler;

public class EnemySpineController : MonoBehaviour
{
    [SerializeField] AnimationReferenceAsset idle, run, walk, Aim, Attack;
    SkeletonAnimation skelAnimation;

    EnemyHandler handler;

    private EnemyShooting shooter;
    private EnemyMelee meleeer;

    Mode previousState;

    [Header("Aim variables")]
    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;

    public float aimOffset = 1;

    private Bone aimBone;
    Vector3 startPos;

    bool wasMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        handler = GetComponentInParent<EnemyHandler>();
        skelAnimation = GetComponent<SkeletonAnimation>();
        previousState = Mode.Idle;

        if (GetComponentInParent<EnemyMelee>() != null)
            meleeer = GetComponentInParent<EnemyMelee>();
        else if (GetComponentInParent<EnemyShooting>() != null)
        {
            shooter = GetComponentInParent<EnemyShooting>();
            aimBone = skelAnimation.Skeleton.FindBone(boneName);
            startPos = aimBone.GetLocalPosition();
        }
        else
            Debug.LogError(transform.parent.gameObject + " has no Ranged Or Melee script on them");


        if (handler == null) Debug.LogError("Handler in " + gameObject.name + " is missing");
    }

    // Update is called once per frame
    void Update()
    {
        var currentState = handler.currentMode;
        if (previousState != currentState || wasMoving != handler.isMoving)
        {
            if (shooter != null)
                PlayNewShooterAnimation();
            else
                PlayNewMeleerAnimation();
        }


        if (shooter != null)
            UpdateTargetLocation();

        previousState = currentState;
        wasMoving = handler.isMoving;
    }

    Vector3 playerLastPosition;
    Vector3 skeletonSpacePoint;
    void UpdateTargetLocation()
    {
        Vector3 aimPosition = transform.position + new Vector3(0, 1.8f);

        if (handler.currentMode == Mode.Aggression)
        {
            playerLastPosition = handler.playerTrans.position;
            skeletonSpacePoint = skelAnimation.transform.InverseTransformPoint(aimPosition + (handler.playerTrans.position - aimPosition).normalized * aimOffset);
        }
        else if (handler.currentMode == Mode.Search)
        {
            skeletonSpacePoint = skelAnimation.transform.InverseTransformPoint(aimPosition + (playerLastPosition - aimPosition).normalized * aimOffset);
        }
        else
        {
            skeletonSpacePoint = startPos;
        }

        skeletonSpacePoint.x *= skelAnimation.Skeleton.ScaleX;
        skeletonSpacePoint.y *= skelAnimation.Skeleton.ScaleY;
        aimBone.SetLocalPosition(skeletonSpacePoint);
    }

    void PlayNewMeleerAnimation()
    {
        var newAnimationState = handler.currentMode;
        Spine.Animation nextAnimation;

        switch (newAnimationState)
        {
            case Mode.Patrol:
                nextAnimation = walk;
                break;
            case Mode.Aggression:
                {
                    if (handler.isMoving)
                        nextAnimation = run;
                    else
                        nextAnimation = idle;

                    break;
                }
            case Mode.Search:
                nextAnimation = run;
                break;
            case Mode.Idle:
                nextAnimation = idle;
                break;
            default:
                Debug.LogError("Invalid Animation State");
                nextAnimation = idle;
                break;
        }

        skelAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
    }

    public void PlayAttackAnimation()
    {
        var shootTrack = skelAnimation.AnimationState.SetAnimation(2, Attack, false);
        shootTrack.AttachmentThreshold = 1f;
        shootTrack.MixDuration = 0f;
        skelAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);
    }

    void PlayNewShooterAnimation()
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
        var aimTrack = skelAnimation.AnimationState.SetAnimation(1, Aim, true);
        aimTrack.AttachmentThreshold = 1f;
        aimTrack.MixDuration = 0f;
    }
    public void StopPlayingAim()
    {
        skelAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);
    }


    private void OnDrawGizmos()
    {
        if (handler != null)
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1.8f) + ((handler.playerTrans.position - (transform.position + new Vector3(0, 1.8f))).normalized) * aimOffset, 0.3f);
    }
}
