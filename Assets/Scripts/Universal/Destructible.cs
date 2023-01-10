using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] GameObject shatter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slice"))
        {
            Destroy(gameObject);

            if (shatter != null)
                Instantiate(shatter, transform.position, transform.rotation);
        }
    }
}
