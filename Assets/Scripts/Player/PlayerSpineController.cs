using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using static PlayerStateHandler;
using static UmbrellaStateHandler;

public class PlayerSpineController : MonoBehaviour
{
    public AnimationReferenceAsset run, idle, jump, descend;
    public AnimationReferenceAsset UmbrellaUp, UmbrellaDown;


    private SkeletonAnimation skeletonAnimation;
    private Slice sliceAction;

    private Rigidbody2D rbody;
    private PlayerStateHandler playerState;
    private UmbrellaStateHandler umbrellaState;
    MovementStates previusPlayerState;
    UmbrellaState previusUmbrellaState;
    bool previusIdleUmbrellaState;

    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponentInParent<PlayerStateHandler>();
        umbrellaState = transform.parent.gameObject.GetComponentInChildren<UmbrellaStateHandler>();

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        sliceAction = GetComponentInParent<Slice>();
        rbody = GetComponentInParent<Rigidbody2D>();
        if (playerState == null) Debug.LogError("Can't find playerStateHandler");

    }

    // Update is called once per frame
    void Update()
    {


        if (sliceAction.isSlicing)
        {
            Turn(sliceAction.sliceDirection < 0);

        }
        else if ((skeletonAnimation.skeleton.ScaleX < 0) != playerState.inputX <= 0)
        {
            Turn(playerState.inputX < 0);
        }

        var currentPlayerState = playerState.currentMoveState;

        var currentUmbrellaState = umbrellaState.currentState;
        bool umbrellaCurrentlyUp = umbrellaState.umbrellaUp;


        if (previusPlayerState != currentPlayerState)
        {
            PlayNewMovementAnimationState();
        }

        if (previusUmbrellaState != currentUmbrellaState || previusIdleUmbrellaState != umbrellaCurrentlyUp)
        {
            PlayUmbrellaStates();
        }

        previusIdleUmbrellaState = umbrellaCurrentlyUp;
        previusUmbrellaState = currentUmbrellaState;
        previusPlayerState = currentPlayerState;
    }

    float animSpeed;
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
            nextAnimation = jump; // Falling animation
                                  //Debug.Log("Is falling");
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
                nextUmbrellaAnimation = UmbrellaUp;
            else
                nextUmbrellaAnimation = UmbrellaDown;
            animSpeed = 1;
        }
        else if (newUmbrellaAnimationState == UmbrellaState.Slash)
        {
            nextUmbrellaAnimation = UmbrellaDown;
            animSpeed = 1;

        }
        else if (newUmbrellaAnimationState == UmbrellaState.Shoot)
        {
            nextUmbrellaAnimation = UmbrellaDown;
            animSpeed = 1;
        }
        else
        {
            nextUmbrellaAnimation = UmbrellaDown;
            animSpeed = 1;
        }

        var umbrellaTrack = skeletonAnimation.AnimationState.SetAnimation(1, nextUmbrellaAnimation, true);
        umbrellaTrack.TimeScale = animSpeed;
        umbrellaTrack.AttachmentThreshold = 1;

    }

    void Turn(bool facingLeft)
    {
        skeletonAnimation.Skeleton.ScaleX = facingLeft ? -1f : 1f;
    }


}
