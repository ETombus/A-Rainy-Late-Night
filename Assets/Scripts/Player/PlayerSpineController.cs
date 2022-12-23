using Spine;
using Spine.Unity;
using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerStateHandler;
using static UmbrellaStateHandler;

public class PlayerSpineController : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] EventDataReferenceAsset attack, footStep;
    public AnimationReferenceAsset run, idle, jump, ascend, descend, landing, damage;
    public AnimationReferenceAsset UmbrellaUp, UmbrellaDown, Slashing, Grappling, Shooting;
    private bool umbrellaAnimLooping = false;

    public AnimationReferenceAsset aimShooting, aimGrappling;


    private SkeletonAnimation skeletonAnimation;
    private Slice sliceAction;

    private Rigidbody2D rbody;

    private PlayerStateHandler playerState;
    MovementStates previusPlayerState;
    bool previusFallingState;

    private UmbrellaStateHandler umbrellaState;
    UmbrellaState previusUmbrellaState;
    bool previusIdleUmbrellaState;

    float animSpeed;

    [Header("Aim variables")]
    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;

    public float aimOffset = 1;
    public float attackAnimTimeOffset = 0.5f;
    public float aimStartHeight = 1.8f;

    private Bone aimBone;
    Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {

        playerState = GetComponentInParent<PlayerStateHandler>();
        umbrellaState = transform.parent.gameObject.GetComponentInChildren<UmbrellaStateHandler>();

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        sliceAction = GetComponentInParent<Slice>();
        rbody = GetComponentInParent<Rigidbody2D>();
        if (playerState == null) Debug.LogError("Can't find playerStateHandler");

        skeletonAnimation.AnimationState.Event += HandleAnimationState;

        aimBone = skeletonAnimation.Skeleton.FindBone(boneName);
        startPos = aimBone.GetLocalPosition();
    }

    void HandleAnimationState(TrackEntry trackEntry, Spine.Event e)
    {
        /*if(e.Data == (attack.EventData))
            handler.PlaySound(handler.thisType);*/
        if (e.Data == (footStep.EventData))
            GetComponentInParent<SpineEventHandler>().Footstep();
    }

    // Update is called once per frame
    void Update()
    {
        if (sliceAction.isSlicing)
        {
            Turn(sliceAction.sliceDirection < 0);
        }
        else if ((skeletonAnimation.skeleton.ScaleX < 0) != !playerState.facingRight)
        {
            Turn(!playerState.facingRight);
        }

        var currentPlayerState = playerState.currentMoveState;

        // MOVEMENT
        if (previusPlayerState != playerState.currentMoveState || previusFallingState != playerState.falling)
        {
            PlayNewMovementAnimationState();
        }

        previusPlayerState = playerState.currentMoveState;
        previusFallingState = playerState.falling;


        // UMBRELLA
        if (previusUmbrellaState != umbrellaState.currentState || previusIdleUmbrellaState != umbrellaState.umbrellaUp)
        {
            PlayUmbrellaStates();
        }

        if (currentPlayerState == MovementStates.Grappling)
            SetAimBone(false);
        else
            SetAimBone(true);

        previusIdleUmbrellaState = umbrellaState.umbrellaUp;
        previusUmbrellaState = umbrellaState.currentState;

    }

    void PlayNewMovementAnimationState()
    {
        var newAnimationState = playerState.currentMoveState;
        Spine.Animation nextAnimation;

        switch (newAnimationState)
        {
            case MovementStates.GroundMoving:
                {
                    nextAnimation = run;
                    //Debug.Log("Is running");
                    animSpeed = 1;
                }
                break;
            case MovementStates.Idle:
                {
                    nextAnimation = idle;
                    //Debug.Log("Is idle");
                    animSpeed = 1;

                }
                break;
            case MovementStates.Knockback:
                {
                    nextAnimation = damage;
                    Debug.Log("Is running");
                    animSpeed = 1;
                }
                break;
            case MovementStates.Jumping:
                {
                    nextAnimation = jump; // Jumping animation
                                          //Debug.Log("Is jumping");
                    animSpeed = 1;
                }
                break;
            default:
                {
                    if (playerState.falling)
                        nextAnimation = descend; // Falling animation
                    else
                        nextAnimation = ascend;

                    animSpeed = 1;
                }
                break;
        }

        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true).TimeScale = animSpeed;
    }

    
    void PlayUmbrellaStates()
    {


        var newUmbrellaAnimationState = umbrellaState.currentState;
        Spine.Animation nextUmbrellaAnimation;

        switch (newUmbrellaAnimationState)
        {
            case UmbrellaState.Idle:
                {
                    if (umbrellaState.umbrellaUp)
                    {
                        umbrellaAnimLooping = true;
                        nextUmbrellaAnimation = UmbrellaUp;
                    }
                    else
                    {
                        umbrellaAnimLooping = true;
                        nextUmbrellaAnimation = UmbrellaDown;
                    }
                    StopPlayingAim();
                    animSpeed = 1;
                }
                break;
            case UmbrellaState.Aiming:
                {
                    SetAimBone(true);
                    umbrellaAnimLooping = true;
                    nextUmbrellaAnimation = aimShooting;
                    //PlayAim(aimShooting);
                    animSpeed = 1;
                }
                break;
            case UmbrellaState.Shoot:
                {
                    umbrellaAnimLooping = true;
                    StopPlayingAim();
                    nextUmbrellaAnimation = UmbrellaDown;
                    animSpeed = 1;
                }
                break;
            case UmbrellaState.Grapple:
                {
                    SetAimBone(false);
                    umbrellaAnimLooping = true;
                    StopPlayingAim();
                    nextUmbrellaAnimation = aimGrappling;
                    animSpeed = 1;
                }
                break;
            case UmbrellaState.Slash:
                {
                    umbrellaAnimLooping = false;
                    StopPlayingAim();
                    nextUmbrellaAnimation = Slashing;
                    animSpeed = 1;
                }
                break;
            default:
                {
                    umbrellaAnimLooping = true;
                    StopPlayingAim();
                    nextUmbrellaAnimation = UmbrellaDown;
                    animSpeed = 1;
                }
                break;
        }

        var umbrellaTrack = skeletonAnimation.AnimationState.SetAnimation(1, nextUmbrellaAnimation, umbrellaAnimLooping);
        umbrellaTrack.TimeScale = animSpeed;
        umbrellaTrack.MixDuration = 0;
        umbrellaTrack.AttachmentThreshold = 1;

    }

    Vector3 skeletonSpacePoint;
    Vector3 worldMousePosition;

    // vvvvv AIM BONE STUFF vvvvv
    void SetAimBone(bool shooting)
    {
        if (!shooting)
        {
            skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(umbrellaState.hookTarget);
        }
        else
        {
            worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(worldMousePosition);
        }


        skeletonSpacePoint.x *= skeletonAnimation.Skeleton.ScaleX;
        skeletonSpacePoint.y *= skeletonAnimation.Skeleton.ScaleY;
        aimBone.SetLocalPosition(skeletonSpacePoint);
    }
    //void PlayAim(AnimationReferenceAsset aimAnimation)
    //{
    //    var aimTrack = skeletonAnimation.AnimationState.SetAnimation(3, aimAnimation, true);
    //    aimTrack.AttachmentThreshold = 1f;
    //    aimTrack.MixDuration = 0f;
    //}
    public void StopPlayingAim()
    {
        skeletonAnimation.state.AddEmptyAnimation(3, 0.5f, 0.1f);
    }

    void Turn(bool facingLeft)
    {
        skeletonAnimation.Skeleton.ScaleX = facingLeft ? -1f : 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(worldMousePosition, 0.3f);
    }
}
