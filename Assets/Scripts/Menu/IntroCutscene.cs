using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] MenuHandler handler;
    public float volume = 1f;

    public void StartGame()
    {
        StartCoroutine(handler.PlayThump());
    }

    private void Update()
    {
        handler.rainVolume = volume; //is built into the animation
    }
}
