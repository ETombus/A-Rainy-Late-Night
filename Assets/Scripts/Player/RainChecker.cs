using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainChecker : MonoBehaviour
{
    [SerializeField] private float maxCheckDistance = 25f;
    [SerializeField] private int ignorePlayerLayer = ~(1 << 2);

    void Update()
    {
        if (Physics2D.Raycast(transform.position, transform.up, maxCheckDistance, ignorePlayerLayer).collider == null)
        {
            //DAMAGE
        }
    }
}
