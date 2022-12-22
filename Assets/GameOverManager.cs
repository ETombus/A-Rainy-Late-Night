using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    CheckpointManager checkPoints;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        checkPoints = GetComponent<CheckpointManager>();

        checkPoints.SetStartPos(player);
    }

    public void PlayerDeath()
    {
        checkPoints.SetPlayerPosition(player);
    }
}
