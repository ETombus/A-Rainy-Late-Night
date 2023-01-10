using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceDeactivator : MonoBehaviour
{
    GameObject player;
    [SerializeField] float distanceToActivate = 37f;
    ParticleSystem thisSystem;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        thisSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        float offset = Mathf.Abs(player.transform.position.x - transform.position.x);

        if (offset < distanceToActivate)
        {
            thisSystem.Play();
        }
        else if (offset > distanceToActivate)
        {
            thisSystem.Pause();
        }
    }
}
