using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestruct : MonoBehaviour
{
    [SerializeField] float timer = 5f;

    private void Update()
    {
        Destroy(gameObject, timer);
    }
}
