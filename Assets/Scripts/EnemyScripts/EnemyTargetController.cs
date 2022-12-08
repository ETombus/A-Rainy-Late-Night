using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetController : MonoBehaviour
{
    [SpineBone(dataField: "skeletonAnimation")]
    public string boneName;
    private SkeletonAnimation skeletonAnimation;


    private Bone aimBone;


    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        aimBone = skeletonAnimation.Skeleton.FindBone(boneName);
    }

    // Update is called once per frame
    void Update()
    {
        
        var skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(Vector3.zero);
        skeletonSpacePoint.x *= skeletonAnimation.Skeleton.ScaleX;
        skeletonSpacePoint.y *= skeletonAnimation.Skeleton.ScaleY;
        aimBone.SetLocalPosition(skeletonSpacePoint);
    }
}
