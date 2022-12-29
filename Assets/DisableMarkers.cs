using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMarkers : MonoBehaviour
{
    [SerializeField] GameObject[] markersToDisable;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("ShowHints", 1) == 1)
        {
            return;
        }
        else if (PlayerPrefs.GetInt("ShowHints") == 0)
        {
            for (int i = 0; i < markersToDisable.Length; i++)
            {
                markersToDisable[i].SetActive(false);
            }
        }
    }
}
