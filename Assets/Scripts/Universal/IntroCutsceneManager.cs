using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutsceneManager : MonoBehaviour
{
    [SerializeField] GameObject introCutsceneMarker;

    [SerializeField] GameObject[] healthbar;

    [SerializeField] GameObject player;

    [SerializeField] GameObject firstMarker;

    private void Start()
    {
        if (PlayerPrefs.GetInt("PlayIntroCutscene", 1) == 1)
        {
            for (int i = 0; i < healthbar.Length; i++) { healthbar[i].SetActive(false); }

            //player.SetActive(false);
            player.GetComponent<PlayerInputHandler>().enabled = false;
            firstMarker.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("PlayIntroCutscene") == 0)
        {
            introCutsceneMarker.SetActive(false);
        }
    }

    public void IntroCutsceneDone()
    {
        for (int i = 0; i < healthbar.Length; i++) { healthbar[i].SetActive(true); }

        //player.SetActive(false);
        player.GetComponent<PlayerInputHandler>().enabled = true;
        firstMarker.SetActive(true);
    }
}
