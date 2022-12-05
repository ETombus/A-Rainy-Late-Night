using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    public Vector2 targetPos;
    [HideInInspector] public float hookSpeed;
    [HideInInspector] public float playerSpeed;
    public GameObject player;

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, targetPos, hookSpeed * Time.deltaTime);

        float dist = Vector2.Distance(transform.position, targetPos);
        if (dist < 1)
            player.transform.position = Vector2.Lerp(player.transform.position, transform.position, hookSpeed * Time.deltaTime);
    }
}
