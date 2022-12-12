using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            try
            {
                collision.gameObject.GetComponent<Healthbar>().ReduceHealth(damage);
            }
            catch
            {
                Debug.Log("Player got no healthScript");
            }

            Destroy(this.gameObject);
        }
    }
}
