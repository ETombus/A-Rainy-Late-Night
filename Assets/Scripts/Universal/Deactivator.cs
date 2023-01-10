using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
