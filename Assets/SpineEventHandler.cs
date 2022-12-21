using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineEventHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem footstep;

    public void Footstep()
    {
        Debug.Log("step");
        footstep.Play();
    }
}
