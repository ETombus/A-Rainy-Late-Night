using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using static PlayerStateHandler;

public class PlayerSpineController : MonoBehaviour
{
    public AnimationReferenceAsset run, idle, jump;
    private SkeletonAnimation skeletonAnimation;


    private PlayerStateHandler stateHandler;
    MovementStates previusState;

    // Start is called before the first frame update
    void Start()
    {
        stateHandler = GetComponentInParent<PlayerStateHandler>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (stateHandler == null) Debug.LogError("Can't find playerStateHandler");


    }

    // Update is called once per frame
    void Update()
    {

        if ((skeletonAnimation.skeleton.ScaleX < 0) != stateHandler.inputX <= 0)
        {  // Detect changes in model.facingLeft
            Turn(stateHandler.inputX < 0);
        }

        var currentState = stateHandler.currentMoveState;

        if (previusState != currentState)
        {
            PlayNewAnimationState();
        }

        previusState = currentState;

    }

    float animSpeed;
    void PlayNewAnimationState()
    {
        var newAnimationState = stateHandler.currentMoveState;
        Spine.Animation nextAnimation;


        if (newAnimationState == MovementStates.Jumping)
        {
            nextAnimation = jump; // Jumping animation
            //Debug.Log("Is jumping");
            animSpeed = 1;
        }
        else
        {
            if (newAnimationState == MovementStates.Idle)
            {
                nextAnimation = idle;
                //Debug.Log("Is idle");
                animSpeed = 1;

            }
            else if(newAnimationState == MovementStates.GroundMoving)
            {
                nextAnimation = run;
                //Debug.Log("Is running");
                animSpeed = 2;
            }
            else
            {
                nextAnimation = jump; // Falling animation
                //Debug.Log("Is falling");
                animSpeed = 1;
            }
        }
        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true).TimeScale = animSpeed;
    }

    void Turn(bool facingLeft)
    {
        skeletonAnimation.Skeleton.ScaleX = facingLeft ? -1f : 1f;
    }


}
