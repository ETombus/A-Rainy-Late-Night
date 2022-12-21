using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            try
            {
                collision.gameObject.GetComponent<Healthbar>().ReduceHealth(damage, false);
            }
            catch
            {
                Debug.Log("Player got no healthScript");
            }

            Destroy(this.gameObject);
        }
    }
}
