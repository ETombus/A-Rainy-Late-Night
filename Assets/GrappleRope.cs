using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleRope : MonoBehaviour
{
    [Header("Rope Animation")]
    [SerializeField] AnimationCurve wavePattern;
    [SerializeField] AnimationCurve ropeOverTime;
    [Range(2.5f, 7.5f)] [SerializeField] float intensityModifier = 5f;
    float startIntensity;
    float x = 0;
    
    [Header("Rope")]
    [SerializeField] Transform hookStart;
    [SerializeField] Transform hook;
    LineRenderer rope;
    Vector2 ropeDirection;
    Vector3[] pointsOnRope = new Vector3[100];
    
    
    void Start()
    {
        rope = GetComponent<LineRenderer>();
        rope.positionCount = pointsOnRope.Length;

        rope.SetPosition(0, hookStart.position);
        rope.SetPosition(99, hook.position);
        
        rope.enabled = false;

        startIntensity=intensityModifier;
    }

    void Update()
    {
        DrawRope();
    }

void DrawRope()
    {
        if(Grapple.onPlayer)
            rope.enabled = false;
        else
            rope.enabled = true;

        if (!Grapple.stuck && !Grapple.extended && Vector2.Distance(hookStart.position,hook.position) <= Grapple.maxDistance-10)
        {
            intensityModifier = startIntensity;
            DrawRopeWaves();
        }
        else
        {
            intensityModifier-=0.1f;
            intensityModifier = Mathf.Clamp(intensityModifier, 0, 50);
            DrawRopeWaves();
        }
    }

    void DrawRopeWaves()
    {
        rope.SetPosition(0, hookStart.position);
        rope.SetPosition(99, hook.position);
        ropeDirection = hook.position - hookStart.position;

        for(float i = 1; i < pointsOnRope.Length - 1; i++)
        {
            x+=10;
            if(x >=100)
                x=0;

            Vector2 waveOffset = Vector2.Perpendicular(ropeDirection.normalized) * wavePattern.Evaluate(x/pointsOnRope.Length) * intensityModifier;
            Vector2 overTimeOffset = waveOffset * (ropeOverTime.Evaluate(i/pointsOnRope.Length)*10);
            Vector2 currentPosition = Vector2.Lerp(hookStart.position, hook.position, i/100) + overTimeOffset;

            rope.SetPosition((int)i, currentPosition);
        }
    }
}
