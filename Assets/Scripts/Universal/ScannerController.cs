using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerController : MonoBehaviour
{
    public static ScannerController Instance;

    public GameObject Player;

    // Activate text

    private void Awake()
    {
        if (ScannerController.Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (Player == null)
            Player = GameObject.FindWithTag("Player");
    }

}
