using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public GameObject player;

    public EnemyMovement movement;
    public EnemyDetect detection;
    public EnemyShooting shooting;
    public EnemyEdgeDetection edgeDetection;

    public enum Mode { Patrol, Aggression, Search, Idle }
    public Mode currentMode;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        movement = GetComponent<EnemyMovement>();
        shooting = GetComponent<EnemyShooting>();

        detection = GetComponentInChildren<EnemyDetect>();
        edgeDetection = GetComponentInChildren<EnemyEdgeDetection>();
    }
}
