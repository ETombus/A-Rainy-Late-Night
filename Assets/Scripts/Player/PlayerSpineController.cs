using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using static PlayerStateHandler;
using static UmbrellaStateHandler;

public class PlayerSpineController : MonoBehaviour
{
    [SerializeField] EventDataReferenceAsset attack, footStep;
    public AnimationReferenceAsset run, idle, jump, ascend, descend, landing;
    public AnimationReferenceAsset UmbrellaUp, UmbrellaDown, Slashing, Grappling, Shooting;
    private bool umbrellaAnimLooping = false;

    public AnimationReferenceAsset GrappleAim, ShootingAim;


    private SkeletonAnimation skeletonAnimation;
    private Slice sliceAction;

    private Rigidbody2D rbody;
    private PlayerStateHandler playerState;
    private UmbrellaStateHandler umbrellaState;
    MovementStates previusPlayerState;
    bool previusFallingState;
    UmbrellaState previusUmbrellaState;
    bool previusIdleUmbrellaState;

    float animSpeed;


    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponentInParent<PlayerStateHandler>();
        umbrellaState = transform.parent.gameObject.GetComponentInChildren<UmbrellaStateHandler>();

        //Time.timeScale = 0.1f;

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        sliceAction = GetComponentInParent<Slice>();
        rbody = GetComponentInParent<Rigidbody2D>();
        if (playerState == null) Debug.LogError("Can't find playerStateHandler");

        skeletonAnimation.AnimationState.Event += HandleAnimationState;
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

        previusIdleUmbrellaState = umbrellaState.umbrellaUp;
        previusUmbrellaState = umbrellaState.currentState;
    }

    void PlayNewMovementAnimationState()
    {
        var newAnimationState = playerState.currentMoveState;
        Spine.Animation nextAnimation;


        if (newAnimationState == MovementStates.Jumping)
        {
            nextAnimation = jump; // Jumping animation
            //Debug.Log("Is jumping");
            animSpeed = 1;
            
        }
        else if (newAnimationState == MovementStates.Idle)
        {
            nextAnimation = idle;
            //Debug.Log("Is idle");
            animSpeed = 1;

        }
        else if (newAnimationState == MovementStates.GroundMoving)
        {
            nextAnimation = run;
            //Debug.Log("Is running");
            animSpeed = 1;
        }
        else
        {
            if (playerState.falling)
                nextAnimation = descend; // Falling animation
            else
                nextAnimation = ascend;

            animSpeed = 1;
        }

        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true).TimeScale = animSpeed;
    }

    void PlayUmbrellaStates()
    {
        var newUmbrellaAnimationState = umbrellaState.currentState;
        Spine.Animation nextUmbrellaAnimation;

        if (newUmbrellaAnimationState == UmbrellaState.Idle)
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
            animSpeed = 1;
        }
        else if (newUmbrellaAnimationState == UmbrellaState.Slash)
        {
            umbrellaAnimLooping = false;
            nextUmbrellaAnimation = Slashing;
            animSpeed = 1;

        }
        else if (newUmbrellaAnimationState == UmbrellaState.Shoot)
        {
            umbrellaAnimLooping = true;
            nextUmbrellaAnimation = UmbrellaDown;
            animSpeed = 1;
        }
        else
        {
            umbrellaAnimLooping = true;
            nextUmbrellaAnimation = UmbrellaDown;
            animSpeed = 1;
        }

        var umbrellaTrack = skeletonAnimation.AnimationState.SetAnimation(1, nextUmbrellaAnimation, umbrellaAnimLooping);
        umbrellaTrack.TimeScale = animSpeed;
        umbrellaTrack.MixDuration = 0;
        umbrellaTrack.AttachmentThreshold = 1;

    }

    void Turn(bool facingLeft)
    {
        skeletonAnimation.Skeleton.ScaleX = facingLeft ? -1f : 1f;
    }


}
