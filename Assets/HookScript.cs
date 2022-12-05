using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    public GameObject player;
    public Vector2 targetPos;

    [HideInInspector] public float hookSpeed;
    [HideInInspector] public float playerSpeed;

    LineRenderer rope;
    public Transform ropeStart;

    private void Start()
    {
        rope = GetComponent<LineRenderer>();
        rope.positionCount = 2;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, hookSpeed * Time.deltaTime);
        Vector2 dir = ropeStart.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle+90);

        rope.SetPosition(0, ropeStart.position);
        rope.SetPosition(1, transform.position);

        if ((Vector2)transform.position == targetPos)
            Debug.Log("(:");
    }
}
