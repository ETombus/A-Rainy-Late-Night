using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEdgeDetection : MonoBehaviour
{
    [SerializeField] float distToCheck;
    [SerializeField] LayerMask detectableLayers;

    public bool DetectEdges()
    {
       RaycastHit2D hit =  Physics2D.Raycast(transform.position, Vector2.down, distToCheck, detectableLayers);

        if(hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
