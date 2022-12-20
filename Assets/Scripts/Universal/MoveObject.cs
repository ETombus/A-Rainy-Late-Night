using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Vector2[] TargetPositions;
    private int index;

    public float transitionSpeedModifier = 0.5f;

    bool moving = false;
    public bool playOnce = false;
    bool playedOnce = false;

    [SerializeField] float timer;

    Vector2 currentTarget;

    private void Start()
    {
        index = 0;
        timer = 10;

        for (int i = 0; i < TargetPositions.Length; i++)
        {
            TargetPositions[i] += (Vector2)transform.position;
        }
    }

    private void Update()
    {
        if (timer <= 1)
        {
            moving = true;
            transform.position = Vector2.Lerp(TargetPositions[index], TargetPositions[index + 1], timer);

            timer += Time.deltaTime * transitionSpeedModifier;
            if(timer >= 1)
                index++;
        }
        else
        {
            moving = false;
        }
    }

    public void GoNextTarget()
    {

        if (index < TargetPositions.Length && !moving && !playedOnce)
        {
            currentTarget = TargetPositions[index + 1];
            timer = 0;

            if (playOnce)
                playedOnce = true;
        }
    }

    public void GoPreviusTarget()
    {
        if (index > 0 && !moving)
        {
            currentTarget = TargetPositions[index - 1];

            index--;
            timer = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //Changes it due to the movePoints getting updated when start playes

        if (!Application.isPlaying)
        {
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                Gizmos.DrawWireSphere(TargetPositions[i] + (Vector2)transform.position, 0.5f);
            }
        }
        else
        {
            for (int x = 0; x < TargetPositions.Length; x++)
            {
                Gizmos.DrawWireSphere(TargetPositions[x], 0.5f);
            }
        }
    }
}
