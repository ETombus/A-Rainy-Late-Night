using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutscene : MonoBehaviour
{
    IntroCutsceneManager intro;

    private void Start()
    {
        intro = GameObject.Find("GameManager").GetComponent<IntroCutsceneManager>();
    }

    public void BeginGame()
    {
        intro.BeginGame();
    }
}
