using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided With Player");
            collision.gameObject.GetComponent<Healthbar>().ReduceHealth(damage);
            
            Destroy(this.gameObject);
        }
    }
}
