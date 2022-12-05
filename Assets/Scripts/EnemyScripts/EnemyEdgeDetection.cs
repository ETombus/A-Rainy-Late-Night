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
            Debug.Log(gameObject.name + " detected ground");
            return true;
        }
        else
        {
            Debug.Log(gameObject.name + "did not detect ground");
            return false;
        }
    }
}
