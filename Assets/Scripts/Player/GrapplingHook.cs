using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private GameObject hook;

    private void Start()
    {
        hook.SetActive(false);
    }

    void Update()
    {
        
    }
}
