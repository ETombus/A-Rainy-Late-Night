using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint[] checkPoints;

    [SerializeField] int furthestCheckpointReached;

    Vector2 startPos;

    private void Start()
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].index = i + 1;
        }
    }

    public void SetStartPos(GameObject player)
    {
        startPos = player.transform.position;
    }

    public void UpdateCheckpoints(int index)
    {
        for (int i = 0; i < index; i++)
        {
            checkPoints[i].reached = true;
        }

        if (index > furthestCheckpointReached)
            furthestCheckpointReached = index;

        PlayerPrefs.SetInt("CheckpointReached", index);
    }

    public void SetPlayerPosition(GameObject player)
    {
        if (PlayerPrefs.GetInt("CheckpointReached") > 0)
            player.transform.position = checkPoints[PlayerPrefs.GetInt("CheckpointReached") - 1].respawnPos;
        else
            player.transform.position = startPos;
    }

    public void SetPlayerPosition(GameObject player, int checkpointToLoad)
    {
        player.transform.position = checkPoints[checkpointToLoad].respawnPos;
    }
}
